import React, {Component} from 'react'

import 'bootstrap/dist/css/bootstrap.css'
import "../componentsStyles/GenreCardStyle.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

class GenreCard extends Component{

    render() {
        const imageSrc = this.props.image ? this.props.image : process.env.PUBLIC_URL + '/pic.jpg';

        return (
            <Link to={`genre/${this.props.id}`} className={`card, cardsize rounded ${this.props.isMovement ? 'movement-border' : 'genre-border'}`}>
                <img className={`card-img-top ${this.props.isBiggerCard ? 'genre-card-img-big' : 'genre-card-img-small'}`} src={imageSrc} alt="Card image cap" />
                <div className="card-body bgblack">
                    <h5 className={`cardtitle ${this.props.isBiggerCard ? '' : 'genre-card-small-font'}`}>{this.props.title}</h5>
                </div>
            </Link>
        );
    }
}

export default GenreCard
/*
<div className="card, cardsize">
                <img className="card-img-top" src={process.env.PUBLIC_URL + '/pic.jpg'} alt="Card image cap" />
                <div className="card-body">
                    <h5 className="card-title">Название карточки</h5>
                    <p className="card-text">Some quick example text to build on the card title and make up the bulk
                        of the card's content.</p>
                    <a href="#" className="btn btn-primary">Переход куда-нибудь</a>
                </div>
            </div>
 */