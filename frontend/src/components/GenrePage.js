import React, {Component} from 'react';
import '../componentsStyles/GenrePage.css';
import GenreService from '../services/GenreService';
import {Loading} from './Loading';
import Painting from './paintings/Painting';

class GenrePage extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0, 0);

        this.state = {
            genre: null,
            isDataLoaded: false
        };
    }

    async componentDidMount() {
        const id = this.props.match.params.id;

        const genre = await GenreService.getGenre(id, this.props.userId);

        this.setState({
            genre: genre,
            isDataLoaded: true
        });
    }

    render() {
        if(this.state.isDataLoaded){
            if(this.state.genre != null){
                const imageSrc = this.state.genre.image ? this.state.genre.image : process.env.PUBLIC_URL + '/pic.jpg';

                const paintings = this.state.genre.paintings != null && this.state.genre.paintings.length > 0 ?
                    this.state.genre.paintings.map((painting) => {
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
                                         changePageTitle={this.props.changePageTitle}/>);
                    }) : <div className={'genre-no-paintings-found'}>No paintings found</div>;

                return(
                    <div className={'genre-page-container'}>
                        <div className={`genre-page-header ${this.state.genre.isMovement ? 'movement' : 'genre'}`}>
                            <div className={`genre-page-image-container`}>
                                <img src={imageSrc} className={'genre-page-image'}/>
                            </div>
                            <div className={'genre-page-title'}>{this.state.genre.title}</div>
                            <div className={'genre-page-description'}>{this.state.genre.description}</div>
                        </div>
                        <div className={'genre-page-paintings'}>
                            {paintings}
                        </div>
                    </div>
                );
            }else{
                return(<div className={'genre-not-found'}>Genre not found</div>);
            }
        }else{
            return <Loading/>;
        }
    }
}

export default GenrePage;