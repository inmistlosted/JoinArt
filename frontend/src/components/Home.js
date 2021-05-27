import React, {Component} from 'react';
import '../componentsStyles/Home.css';
import Paintings from './paintings/Paintings';
import AuctionService from '../services/AuctionService';
import PaintingService from '../services/PaintingService';
import AlbumService from '../services/AlbumService';
import GenreService from '../services/GenreService';
import {Loading} from './Loading';
import Painting from './paintings/Painting';
import BidCard from './BidCard';
import AlbumCardBig from './AlbumCardBig';
import GenreCard from './GenreCard';

class Home extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0,0);

        this.state = {
            topPaintings: [],
            topAuctions: [],
            topAlbums: [],
            topGenres: [],
            allPaintings: [],
            isDataLoaded: false
        }
    }

    async componentDidMount() {
        const topPaintings = await PaintingService.getTopPaintings(this.props.userId);
        const topAuction = await AuctionService.getTopAuctions();
        const topAlbums = await AlbumService.getTopAlbums();
        const topGenresAndMovements = await GenreService.getTopGenresAndMovements();
        const allPaintings = await PaintingService.getAllPaintings(this.props.userId);

        this.setState({
            topPaintings: topPaintings,
            topAuctions: topAuction,
            topAlbums: topAlbums,
            topGenresAndMovements: topGenresAndMovements,
            allPaintings: allPaintings,
            isDataLoaded: true
        });
    }

    render() {
        if(this.state.isDataLoaded){
            const topPaintings = this.state.topPaintings != null && this.state.topPaintings.length > 0 ?
                this.state.topPaintings.map((painting) => {
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
                                     changePageTitle={this.props.changePageTitle}
                                     isSmaller={true}/>);
                }) : '';

            const topAuctions = this.state.topAuctions.map((auction) => {
                return(
                    <div key={auction.bidId} className="col-md-2">
                        <BidCard id={auction.bidId} image={auction.paintingImage} status={auction.status} price={auction.currentPrice} isBiggerCard={false} />
                    </div>
                );
            });

            const topAlbums = this.state.topAlbums.map((album) => {
                return(
                    <div key={album.albumId} className="col-md-2">
                        <AlbumCardBig id={album.albumId} title={album.title} image={album.image} likesCount={album.likesCount} isBiggerCard={false} />
                    </div>
                );
            });

            const topGenresAndMovements = this.state.topGenresAndMovements.map((genre) => {
                return(
                    <div key={genre.genreId} className="col-md-2">
                        <GenreCard id={genre.genreId} title={genre.title} image={genre.image} isMovement={genre.isMovement} isBiggerCard={false} />
                    </div>
                );
            });

            const allPaintings = this.state.allPaintings != null && this.state.allPaintings.length > 0 ?
                this.state.allPaintings.map((painting) => {
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
                }) : '';

            return (
                <div className="container">
                    <div className="row ">
                        <div className="col-md-12 home-top-title">
                            <h3>Top paintings</h3>
                        </div>
                    </div>
                    <div className="row ">
                        {topPaintings}
                    </div>
                    <div className="row ">
                        <div className="col-md-12 home-mid-title">
                            <h4>Top auctions</h4>
                        </div>
                    </div>
                    <div className="row ">
                        {topAuctions}
                    </div>
                    <div className="row ">
                        <div className="col-md-12 home-mid-title">
                            <h4>Top albums</h4>
                        </div>
                    </div>
                    <div className="row ">
                        {topAlbums}
                    </div>
                    <div className="row ">
                        <div className="col-md-12 home-mid-title">
                            <h4>Top genres and movements</h4>
                        </div>
                    </div>
                    <div className="row ">
                        {topGenresAndMovements}
                    </div>
                    <div className="row home-all-paints">
                        <div className="col-md-12 home-all-paints-title">
                            <h4>All paintings</h4>
                        </div>
                        {allPaintings}
                    </div>
                </div>
            );
        }else{
            return <Loading/>
        }
    }
}

export default Home;