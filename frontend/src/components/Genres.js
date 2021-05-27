import React, {Component} from 'react'
import GenreCard from './GenreCard'
import "../componentsStyles/GenresStyle.css"

import GenreService from '../services/GenreService';
import {Loading} from './Loading';

class Genres extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0,0);

        this.state = {
            allGenresAndMovements: [],
            topGenres: [],
            topMovements: [],
            isDataLoaded: false
        }
    }

    async componentDidMount() {
        const allGenresAndMovements = await GenreService.getAllGenresAndMovements();
        const topGenres = await GenreService.getTopGenres();
        const topMovements = await GenreService.getTopMovements();

        this.setState({
            allGenresAndMovements: allGenresAndMovements,
            topGenres: topGenres,
            topMovements: topMovements,
            isDataLoaded: true
        });
    }

    render() {
        const topGenres = this.state.topGenres.map((genre) => {
            return(
                <div key={genre.genreId} className="col-md-2">
                    <GenreCard id={genre.genreId} title={genre.title} image={genre.image} isMovement={genre.isMovement} isBiggerCard={false} />
                </div>
            );
        });

        const topMovements = this.state.topMovements.map((genre) => {
            return(
                <div key={genre.genreId} className="col-md-2">
                    <GenreCard id={genre.genreId} title={genre.title} image={genre.image} isMovement={genre.isMovement} isBiggerCard={false} />
                </div>
            );
        });

        const allGenres = this.state.allGenresAndMovements.map((genre) => {
            return(
                <div key={genre.genreId} className="col-md-3">
                    <GenreCard id={genre.genreId} title={genre.title} image={genre.image} isMovement={genre.isMovement} isBiggerCard={true} />
                </div>
            );
        });

        if(this.state.isDataLoaded){
            return (
                <div className="container">
                    <div className="row ">
                        <div className="col-md-12 toptopicstitle ">
                            <h4>Top genres</h4>
                        </div>
                    </div>
                    <div className="row ">
                        {topGenres}
                    </div>
                    <div className="row ">
                        <div className="col-md-12 top-movements-title">
                            <h4>Top movements</h4>
                        </div>
                    </div>
                    <div className="row ">
                        {topMovements}
                    </div>
                    <div className="row ">
                        <div className="col-md-12 alltopicstitle">
                            <h4>All genres and movements</h4>
                        </div>
                        {allGenres}
                    </div>
                </div>
            );
        }else{
            return <Loading/>
        }
    }
}

export default Genres;