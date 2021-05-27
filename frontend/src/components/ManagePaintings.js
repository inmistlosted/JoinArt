import React, {Component} from 'react'
import "../componentsStyles/ManagePaintings.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    Redirect
} from "react-router-dom";
import PaintingService from '../services/PaintingService';
import Painting from './paintings/Painting';
import AddPainting from './AddPainting';
import EditPainting from './EditPainting';
import {Loading} from './Loading';

class ManagePaintings extends Component{
    constructor(props) {
        super(props);

        this.state = {
            isDataLoaded: false,
            paintings: null,

            isEditPopupOpened: false,
            editId: 0,
            editTitle: '',
            editMaterials: '',
            editPainter: '',
            editPrice: 0,
            editDescr: '',
            editGenreId: 0,
            editImage: null,
            editImagePreview: null
        };
    }

    async componentDidMount() {
        const paintings = await PaintingService.getUserPaintings(this.props.userId, 0);

        this.setState({
            paintings: paintings,
            isDataLoaded: true
        });
    }

    deletePainting = async (paintingId) => {
        await PaintingService.deletePainting(paintingId);

        const newPaintingsArray = [];

        this.state.paintings.forEach(painting => {
            if(painting.paintingId !== paintingId){
                newPaintingsArray.push(painting);
            }
        });

        this.setState({
            paintings: newPaintingsArray
        })
    }

    openEditForm = (paintingId) => {
        let currentPainting = null;

        this.state.paintings.forEach(painting => {
            if(painting.paintingId === paintingId){
                currentPainting = painting;
            }
        });

        this.setState({
            editId: currentPainting.paintingId,
            editTitle: currentPainting.title,
            editMaterials: currentPainting.materials,
            editPainter: currentPainting.painter,
            editPrice: currentPainting.price,
            editDescr: currentPainting.description,
            editImagePreview: currentPainting.imagePath,
            editGenreId: currentPainting.genres && currentPainting.genres.length > 0 ? currentPainting.genres[0].genreId : 1,
            isEditPopupOpened: true
        });
    }

    keepEditPopup = (e) => {
        e.stopPropagation();
    }

    closeEditPopup = () => {
        this.setState({isEditPopupOpened: false});
    }

    handleEditTitleChange = (e) => {
        this.setState({editTitle: e.target.value});
    }

    handleEditMaterialsChange = (e) => {
        this.setState({editMaterials: e.target.value});
    }

    handleEditPainterChange = (e) => {
        this.setState({editPainter: e.target.value});
    }

    handleEditPriceChange = (e) => {
        this.setState({editPrice: +e.target.value});
    }

    handleEditDescrChange = (e) => {
        this.setState({editDescr: e.target.value});
    }

    handleEditGenreIdChange = (e) => {
        this.setState({editGenreId: e.target.value});
    }

    handleEditImageChange = (e) => {
        const image = e.target.files[0];
        const reader = new FileReader();

        reader.onload = () => {
            this.setState({editImage: image, editImagePreview: reader.result});
        };

        reader.readAsDataURL(image);
    }

    editPainting = async () => {
        window.scrollTo(0, 0);

        await PaintingService.updatePainting(
            this.state.editId,
            this.state.editTitle,
            this.state.editMaterials,
            this.state.editPainter,
            this.state.editPrice,
            this.state.editDescr,
            this.state.editGenreId,
            this.state.editImage
        );

        const newPaintingsArray = [];

        this.state.paintings.forEach(painting => {
            if(painting.paintingId === this.state.editId){
                const paint = {
                    paintingId: this.state.editId,
                    title: this.state.editTitle,
                    materials: this.state.editMaterials,
                    painter: this.state.editPainter,
                    price: this.state.editPrice,
                    description: this.state.editDescr,
                    imagePath: this.state.editImagePreview == null ? painting.imagePath : this.state.editImagePreview,
                    likesCount: painting.likesCount
                };

                newPaintingsArray.push(paint);
            }else{
                newPaintingsArray.push(painting);
            }
        });

        this.setState({
            paintings: newPaintingsArray,
            isEditPopupOpened: false,
            editId: 0,
            editTitle: '',
            editMaterials: '',
            editPainter: '',
            editPrice: 0,
            editDescr: '',
            editGenreId: 0,
            editImage: null,
            editImagePreview: null
        });
    }

    render() {
        if(this.props.userId !== 0){
            if(this.state.isDataLoaded){
                const paintings = this.state.paintings != null && this.state.paintings.length > 0
                    ? this.state.paintings.map((painting) => {
                        return(
                            <div key={painting.paintingId} className={'manage-painting-item'}>
                                <div className={'item image'}>
                                    <img src={painting.imagePath}/>
                                </div>
                                <div className={'item title'}>{painting.title}</div>
                                <div className={'item descr'}>{painting.description}</div>
                                <div className={'item price'}>${painting.price}</div>
                                <div className={'item likes-count'}>{painting.likesCount}</div>
                                <div className={'item edit'} >
                                    <i className={'fa fa-edit'} onClick={() => this.openEditForm(painting.paintingId)}/>
                                </div>
                                <div className={'item delete'} >
                                    <i className={'fa fa-trash'} onClick={() => this.deletePainting(painting.paintingId)}/>
                                </div>
                            </div>
                        );
                    })
                    : <div className={'genre-no-paintings-found'}>No paintings found</div>;

                const editPopup = this.state.isEditPopupOpened ? <EditPainting
                    closePopup={this.closeEditPopup}
                    keepPopup={this.keepEditPopup}
                    title={this.state.editTitle}
                    handleTitle={this.handleEditTitleChange}
                    materials={this.state.editMaterials}
                    handleMaterials={this.handleEditMaterialsChange}
                    description={this.state.editDescr}
                    handleDescription={this.handleEditDescrChange}
                    painter={this.state.editPainter}
                    handlePainter={this.handleEditPainterChange}
                    price={this.state.editPrice}
                    handlePrice={this.handleEditPriceChange}
                    handleGenre={this.handleEditGenreIdChange}
                    handleImage={this.handleEditImageChange}
                    image={this.state.editImagePreview}
                    editPainting={this.editPainting}/> : '';

                return (
                    <div className={`manage-paintings ${this.state.isEditPopupOpened ? 'manage-paintings-mrg' : ''}`}>
                        {editPopup}
                        <div className={'manage-paintings-title'}>Manage paintings</div>
                        <div className={'manage-paintings-container'}>
                            <div className={'manage-paintings-controls'}>
                                <div className={'manage-paintings-sort'}>
                                    Sort by:
                                    <select>
                                        <option>Cheaper first</option>
                                        <option>Expensive first</option>
                                        <option>Title</option>
                                        <option>Popularity</option>
                                    </select>
                                </div>
                            </div>
                            <div className={'manage-paintings-paints'}>
                                <div className={'manage-painting-item'}>
                                    <div className={'ttl item image'}>Image</div>
                                    <div className={'ttl item title'}>Title</div>
                                    <div className={'ttl item descr'}>Description</div>
                                    <div className={'ttl item price'}>Price</div>
                                    <div className={'ttl item likes-count'}>Likes count</div>
                                    <div className={'ttl item edit'} />
                                    <div className={'ttl item delete'} />
                                </div>
                                {paintings}
                            </div>
                        </div>
                    </div>
                );
            }else{
                return <Loading />;
            }
        }else{
            return <Redirect to={'/login'}/>;
        }
    }
}

export default ManagePaintings;