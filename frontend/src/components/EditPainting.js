import React, {Component} from 'react';
import '../componentsStyles/AddPainting.css';
import GenreService from '../services/GenreService';

class EditPainting extends Component{
    constructor(props) {
        super(props);

        this.state = {
            content: '',
            isValidTitle: true,
            isValidPainter: true,
            isValidPrice: true,
            genres: []
        };
    }

    async componentDidMount() {
        const genres = await GenreService.getAllGenresAndMovements();

        this.setState({genres: genres});
    }

    checkTitleValid = () => {
        const title = this.props.title;

        if(title.length < 3){
            this.setState({isValidTitle: false});
        }else{
            this.setState({isValidTitle: true});
        }
    }

    checkPainterValid = () => {
        const painter = this.props.painter;

        if(painter.length < 3){
            this.setState({isValidPainter: false});
        }else{
            this.setState({isValidPainter: true});
        }
    }

    checkPriceValid = () => {
        const price = this.props.price;

        if(price < 10){
            this.setState({isValidPrice: false});
        }else{
            this.setState({isValidPrice: true});
        }
    }

    render() {
        const genres = this.state.genres.map(genre => {
            return (
                <option value={genre.genreId}>{genre.title}</option>
            );
        });

        return(
            <div className={'add-painting-container'} onClick={this.props.closePopup}>
                <div className={'add-painting'} onClick={this.props.keepPopup}>
                    <div className={'add-comment-form-title'}>
                        Edit painting
                    </div>
                    <div className="add-paint-input">
                        Title
                        <input type="text" className={`form-control signup-input ${this.state.isValidTitle ? '' : 'signup-input-error'}`} placeholder="Title" value={this.props.title} onChange={this.props.handleTitle} onBlur={this.checkTitleValid}/>
                        {this.state.isValidTitle ? '' : <div className={'signup-error-message'}>Title should contain more than 2 characters</div>}
                    </div>
                    <div className="add-paint-input">
                        Painter
                        <input type="text" className={`form-control signup-input ${this.state.isValidPainter ? '' : 'signup-input-error'}`} placeholder="Painter" value={this.props.painter} onChange={this.props.handlePainter} onBlur={this.checkPainterValid}/>
                        {this.state.isValidPainter ? '' : <div className={'signup-error-message'}>Painter name should contain more than 2 characters</div>}
                    </div>
                    <div className="add-paint-input">
                        Price
                        <input type="text" className={`form-control signup-input ${this.state.isValidPrice ? '' : 'signup-input-error'}`} placeholder="Painter" value={this.props.price} onChange={this.props.handlePrice} onBlur={this.checkPriceValid}/>
                        {this.state.isValidPrice ? '' : <div className={'signup-error-message'}>Price should be greater than $10</div>}
                    </div>
                    <div className="add-paint-input">
                        Materials
                        <textarea rows={5} className={`form-control signup-input`} placeholder="Materials" value={this.props.materials} onChange={this.props.handleMaterials}/>
                    </div>
                    <div className="add-paint-input">
                        Description
                        <textarea rows={10} className={`form-control signup-input`} placeholder="Description" value={this.props.description} onChange={this.props.handleDescription}/>
                    </div>
                    <div className="add-paint-input">
                        Genre
                        <select onChange={this.props.handleGenre}>
                            {genres}
                        </select>
                    </div>
                    <div className="add-paint-input">
                        Image
                        <img className={'edit-painting-image'} src={this.props.image}/>
                        <input type={'file'} accept="image/*" className={`form-control signup-input`} placeholder="Image" onChange={this.props.handleImage}/>
                    </div>
                    <div className={'add-comment-btn'} onClick={this.props.editPainting}>Edit painting</div>
                </div>
            </div>
        );
    }
}

export default EditPainting;