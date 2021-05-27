import React, {Component} from 'react';
import '../componentsStyles/AddToAlbum.css';
import AlbumCard from './AlbumCard';
import AlbumService from '../services/AlbumService';
import {Loading} from './Loading';
import CreateAlbum from './CreateAlbum';

class AlbumsPopup extends Component{
    constructor(props) {
        super(props);

        this.state = {
            isDataLoaded: false,
            albums: [],
            showCreateAlbumPopup: false,
            showEditAlbumPopup: false,
            newAlbumTitle: '',
            newAlbumDescription: '',
            newAlbumError: false,
            editAlbumId: 0,
            editAlbumTitle: '',
            editAlbumDescription: '',
            editAlbumError: false,
        };
    }

    async componentDidMount() {
        let albums = await AlbumService.getUserAllAlbums(this.props.userId);

        if(!albums){
            albums = [];
        }

        this.setState({isDataLoaded: true, albums: albums});
    }

    openCreateAlbumPopup = () => {
        window.scrollTo(0, 0);
        this.setState({showCreateAlbumPopup: true});
    }

    handleNewAlbumTitleChange = (e) => {
        this.setState({newAlbumTitle: e.target.value});
    }

    handleNewAlbumDescChange = (e) => {
        this.setState({newAlbumDescription: e.target.value});
    }

    closeCreateAlbumPopup = (e) => {
        this.setState({showCreateAlbumPopup: false});
    }

    keepCreateAlbumPopup = (e) => {
        e.stopPropagation();
    }

    createAlbum = async () => {
        if(this.state.newAlbumTitle.length === 0){
            this.setState({newAlbumError: true});
        }else{
            const albumResponse = await AlbumService.create(this.state.newAlbumTitle, this.state.newAlbumDescription, this.props.userId);

            this.setState({
                newAlbumError: false,
                showCreateAlbumPopup: false,
                albums: [...this.state.albums, {
                    albumId: albumResponse.albumId,
                    title: this.state.newAlbumTitle,
                    description: this.state.newAlbumDescription
                }],
                newAlbumTitle: '',
                newAlbumDescription: '',
            });
        }
    }

    deleteAlbum = async (albumId) => {
        await AlbumService.delete(albumId);

        const newAlbumsArr = [];

        this.state.albums.forEach(album => {
            if(album.albumId !== albumId){
                newAlbumsArr.push(album);
            }
        });

        this.setState({albums: newAlbumsArr});
    }

    openEditAlbumPopup = (id, title, description) => {
        window.scrollTo(0, 0);
        this.setState({showEditAlbumPopup: true, editAlbumId: id, editAlbumTitle: title, editAlbumDescription: description});
    }

    handleEditAlbumTitleChange = (e) => {
        this.setState({editAlbumTitle: e.target.value});
    }

    handleEditAlbumDescChange = (e) => {
        this.setState({editAlbumDescription: e.target.value});
    }

    closeEditAlbumPopup = () => {
        this.setState({showEditAlbumPopup: false});
    }

    keepEditAlbumPopup = (e) => {
        e.stopPropagation();
        this.setState({showEditAlbumPopup: true});
    }

    editAlbum = async () => {
        if(this.state.editAlbumTitle.length === 0){
            this.setState({editAlbumError: true});
        }else{
            await AlbumService.update(this.state.editAlbumId, this.state.editAlbumTitle, this.state.editAlbumDescription);

            const newAlbumsArray = [];

            this.state.albums.forEach(album => {
                if(album.albumId === this.state.editAlbumId){
                    const editedAlbum = {
                        albumId: album.albumId,
                        title: this.state.editAlbumTitle,
                        description: this.state.editAlbumDescription,
                        image: album.image
                    };

                    newAlbumsArray.push(editedAlbum);
                }else{
                    newAlbumsArray.push(album);
                }
            });

            this.setState({
                editAlbumError: false,
                showEditAlbumPopup: false,
                albums: newAlbumsArray,
                editAlbumId: 0,
                editAlbumTitle: '',
                editAlbumDescription: '',
            });
        }
    }

    render() {
        if(this.state.isDataLoaded){
            const albums = this.state.albums.map((album) => {
                return(<AlbumCard
                    key={album.albumId}
                    id={album.albumId}
                    image={album.image}
                    title={album.title}
                    description={album.description}
                    chooseAlbum={this.props.chooseAlbum}
                    fromUserPage={true} delete={this.deleteAlbum} openEdit={this.openEditAlbumPopup}/>);
            });

            const createAlbum = this.state.showCreateAlbumPopup
                ? <CreateAlbum
                    closePopup={this.closeCreateAlbumPopup}
                    keepPopup={this.keepCreateAlbumPopup}
                    title={this.state.newAlbumTitle}
                    description={this.state.newAlbumDescription}
                    error={this.state.newAlbumError}
                    handleTitle={this.handleNewAlbumTitleChange}
                    handleDesc={this.handleNewAlbumDescChange}
                    create={this.createAlbum}/> : '';

            const editAlbum = this.state.showEditAlbumPopup
                ? <CreateAlbum
                    closePopup={this.closeEditAlbumPopup}
                    keepPopup={this.keepEditAlbumPopup}
                    title={this.state.editAlbumTitle}
                    description={this.state.editAlbumDescription}
                    error={this.state.editAlbumError}
                    handleTitle={this.handleEditAlbumTitleChange}
                    handleDesc={this.handleEditAlbumDescChange}
                    create={this.editAlbum}
                    isEdit={true}/> : '';

            return(
                <div className={'add-to-album-form-container'} onClick={this.props.closePopup}>
                    <div className={'add-to-album-inner'} onClick={this.props.keepPopup}>
                        {createAlbum}
                        {editAlbum}
                        <div className={'add-to-album-title'}>Albums</div>
                        <div className={'add-to-album-albums'}>
                            {albums}
                            <div className={'add-to-album-create'} onClick={this.openCreateAlbumPopup}>
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

export default AlbumsPopup;