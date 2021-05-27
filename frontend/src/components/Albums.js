import React, {Component} from 'react'
import "../componentsStyles/Albums.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

import {Loading} from './Loading';
import AlbumCardBig from './AlbumCardBig';
import AlbumService from '../services/AlbumService';

class Albums extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0,0);

        this.state = {
            allAlbums: [],
            topAlbums: [],
            isDataLoaded: false
        }
    }

    async componentDidMount() {
        const allAlbums = await AlbumService.getAllAlbums();
        const topAlbums = await AlbumService.getTopAlbums();

        this.setState({
            allAlbums: allAlbums,
            topAlbums: topAlbums,
            isDataLoaded: true
        });
    }

    render() {
        const topAlbums = this.state.topAlbums.map((album) => {
            return(
                <div key={album.albumId} className="col-md-2">
                    <AlbumCardBig id={album.albumId} title={album.title} image={album.image} likesCount={album.likesCount} isBiggerCard={false} />
                </div>
            );
        });

        const allAlbums = this.state.topAlbums.map((album) => {
            return(
                <div key={album.albumId} className="col-md-3">
                    <AlbumCardBig id={album.albumId} title={album.title} image={album.image} likesCount={album.likesCount} isBiggerCard={true} />
                </div>
            );
        });

        if(this.state.isDataLoaded){
            return (
                <div className="container">
                    <div className="row ">
                        <div className="col-md-12 toptopicstitle">
                            <h4>Top albums</h4>
                        </div>
                    </div>
                    <div className="row ">
                        {topAlbums}
                    </div>
                    <div className="row ">
                        <div className="col-md-12 alltopicstitle">
                            <h4>All albums</h4>
                        </div>
                        {allAlbums}
                    </div>
                </div>
            );
        }else{
            return <Loading/>
        }
    }
}


export default Albums