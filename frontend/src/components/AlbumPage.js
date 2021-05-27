import React, {Component} from 'react';
import '../componentsStyles/GenrePage.css';
import GenreService from '../services/GenreService';
import {Loading} from './Loading';
import Painting from './paintings/Painting';
import AlbumService from '../services/AlbumService';

class AlbumPage extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0, 0);

        this.state = {
            album: null,
            paintings: [],
            likesCount: 0,
            isDataLoaded: false
        };
    }

    async componentDidMount() {
        const id = this.props.match.params.id;

        const album = await AlbumService.getAlbum(id, this.props.userId);

        this.setState({
            album: album,
            paintings: album.paintings,
            likesCount: album.likesCount,
            isDataLoaded: true
        });
    }

    removeFromAlbum = async (id) => {
        await AlbumService.removeFromAlbum(this.state.album.albumId, id);

        let paintingLikesCount = 0;
        for(let i = 0; i < this.state.paintings.length; i++){
            if(this.state.paintings[i].paintingId === id){
                paintingLikesCount = this.state.paintings[i].likesCount;
            }
        }

        this.setState({
            likesCount: this.state.likesCount - paintingLikesCount,
            paintings: this.state.paintings.filter((painting) => { return painting.paintingId !== id })
        });
    }

    render() {
        if(this.state.isDataLoaded){
            if(this.state.album != null){
                const imageSrc = this.state.album.image ? this.state.album.image : process.env.PUBLIC_URL + '/pic.jpg';

                const paintings = this.state.paintings != null && this.state.paintings.length > 0 ?
                    this.state.paintings.map((painting) => {
                        return(<Painting key={painting.paintingId}
                                         id={painting.paintingId}
                                         image={painting.imagePath}
                                         title={painting.title}
                                         author={painting.painter}
                                         status={painting.status}
                                         price={painting.price}
                                         likesCount={painting.likesCount}
                                         isLiked={painting.isLiked}
                                         commentsCount={painting.commentsCount}
                                         userId={this.props.userId}
                                         belongsToUser={this.state.album.belongsToUser}
                                         removeFromAlbum={this.removeFromAlbum}
                                         changePageTitle={this.props.changePageTitle}/>);
                    }) : <div className={'genre-no-paintings-found'}>No paintings found</div>;

                return(
                    <div className={'genre-page-container'}>
                        <div className={`genre-page-header album-page-border`}>
                            <div className={`genre-page-image-container`}>
                                <img src={imageSrc} className={'genre-page-image'}/>
                            </div>
                            <div className={'genre-page-title'}>{this.state.album.title}</div>
                            <div className={'genre-page-description'}>{this.state.album.description}</div>
                            <div className={'album-page-likes'}>{this.state.likesCount} likes</div>
                        </div>
                        <div className={'genre-page-paintings'}>
                            {paintings}
                        </div>
                    </div>
                );
            }else{
                return(<div className={'genre-not-found'}>Album not found</div>);
            }
        }else{
            return <Loading/>;
        }
    }
}

export default AlbumPage;