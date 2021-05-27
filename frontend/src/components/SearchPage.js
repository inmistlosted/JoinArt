import React, {Component} from 'react';
import '../componentsStyles/SearchPage.css';
import GenreService from '../services/GenreService';
import {Loading} from './Loading';
import Painting from './paintings/Painting';
import AlbumService from '../services/AlbumService';
import PaintingService from '../services/PaintingService';

class SearchPage extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0, 0);

        this.state = {
            query: '',
            paintings: [],
            isDataLoaded: false
        };
    }

    async componentDidMount() {
        const query = this.getSearchQuery(this.props.location.search);
        const paintings = await PaintingService.searchPaintings(query, this.props.userId);

        this.setState({
            query: query,
            paintings: paintings,
            isDataLoaded: true
        });
    }

    getSearchQuery(query){
        const searchQueryKeyWord = 'query';
        const params = query.split('&');
        let searchQuery = '';

        params.forEach((param) => {
            if(param.includes(searchQueryKeyWord)){
                searchQuery = param.split('=')[1];
            }
        });

        return searchQuery;
    }

    render() {
        if(this.state.isDataLoaded){
            if(this.state.query.length > 0){
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
                                         changePageTitle={this.props.changePageTitle}/>);
                    }) : <div className={'genre-no-paintings-found'}>No paintings found</div>;

                return(
                    <div className={'search-page-container'}>
                        <div className={'search-page-query-title'}>
                            Search results for <span className={'search-page-query'}>"{this.state.query}"</span>
                        </div>
                        <div className={'search-page-paintings'}>
                            {paintings}
                        </div>
                    </div>
                );
            }else{
                return(<div className={'search-not-found'}>Error in search query</div>);
            }
        }else{
            return <Loading/>;
        }
    }
}

export default SearchPage;