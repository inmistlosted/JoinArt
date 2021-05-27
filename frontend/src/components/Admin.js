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
import GenreService from '../services/GenreService';

class Admin extends Component{
    constructor(props) {
        super(props);

        this.state = {
            isDataLoaded: false,
            genres: null,
        };
    }

    async componentDidMount() {
        const genres = await GenreService.getTopGenresAndMovements();

        this.setState({
            genres: genres,
            isDataLoaded: true
        });
    }

    render() {
        if(this.props.userId !== 0){
            if(this.state.isDataLoaded){
                const genres = this.state.genres != null && this.state.genres.length > 0
                    ? this.state.genres.map((genre) => {
                        return(
                            <div key={genre.genreId} className={'manage-painting-item'}>
                                <div className={'item image gr'}>
                                    <img src={genre.image}/>
                                </div>
                                <div className={'item title gr'}>{genre.title}</div>
                                <div className={'item descr gr'}>{genre.description}</div>
                                <div className={'item edit gr'} >
                                    <i className={'fa fa-edit'}/>
                                </div>
                                <div className={'item delete gr'} >
                                    <i className={'fa fa-trash'}/>
                                </div>
                            </div>
                        );
                    })
                    : <div className={'genre-no-paintings-found'}>No genres found</div>;

                return (
                    <div className={'admin-container'}>
                        <div className={'charts-btn-container'}>
                            <div className={'charts-btn-inner'}>
                                <div className={'charts-btn'}>Paintings</div>
                                <div className={'charts-btn active'}>Genres and movements</div>
                                <div className={'charts-btn'}>Auctions</div>
                                <div className={'charts-btn'}>Albums</div>
                                <div className={'charts-btn'}>Users</div>
                            </div>
                        </div>
                        <div className={`manage-paintings ${this.state.isEditPopupOpened ? 'manage-paintings-mrg' : ''} admin-outer`}>
                            <div className={'manage-paintings-title gr-ttl'}>
                                Manage genres and movements
                                <div className={'add-gr'}>Add genre</div>
                            </div>
                            <div className={'manage-paintings-container'}>
                                <div className={'manage-paintings-paints'}>
                                    <div className={'manage-painting-item'}>
                                        <div className={'ttl item image gr'}>Image</div>
                                        <div className={'ttl item title gr'}>Title</div>
                                        <div className={'ttl item descr gr'}>Description</div>
                                        <div className={'ttl item edit gr'} />
                                        <div className={'ttl item delete gr'} />
                                    </div>
                                    {genres}
                                </div>
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

export default Admin;