import React, {Component} from 'react';
import '../componentsStyles/AddToAlbum.css';
import AlbumCard from './AlbumCard';
import AlbumService from '../services/AlbumService';
import {Loading} from './Loading';

class AddToAlbum extends Component{
    constructor(props) {
        super(props);

        this.state = {
            isDataLoaded: false,
            albums: []
        };
    }

    async componentDidMount() {
        let albums = await AlbumService.getUserAlbums(this.props.userId, this.props.paintingId);

        if(!albums){
            albums = [];
        }

        this.setState({isDataLoaded: true, albums: albums});
    }

    render() {
        if(this.state.isDataLoaded){
            const albums = this.state.albums.map((album) => {
                return(<AlbumCard key={album.albumId} id={album.albumId} image={album.image} title={album.title} chooseAlbum={this.props.chooseAlbum} isAdded={album.isPaintingInAlbum}/>);
            });

            return(
                <div className={'add-to-album-form-container'} onClick={this.props.closePopup}>
                    <div className={'add-to-album-inner'} onClick={this.props.keepPopup}>
                        <div className={'add-to-album-title'}>Choose album</div>
                        <div className={'add-to-album-albums'}>
                            {albums}
                            <div className={'add-to-album-create'} onClick={this.props.openCreate}>
                                <div className={'add-to-album-create-title'}>Create new album...</div>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }else{
            return (
                <div className={'add-to-album-form-container'} onClick={this.props.closePopup}>
                    <div className={'add-to-album-inner'} onClick={this.props.keepPopup}>
                        <Loading />
                    </div>
                </div>
            );
        }
    }
}

export default AddToAlbum;