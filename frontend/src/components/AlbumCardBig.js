import React, {Component} from 'react'

import 'bootstrap/dist/css/bootstrap.css'
import "../componentsStyles/GenreCardStyle.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

class AlbumCardBig extends Component{

    render() {
        const imageSrc = this.props.image ? this.props.image : process.env.PUBLIC_URL + '/pic.jpg';

        return (
            <Link to={`album/${this.props.id}`} className={`card, cardsize rounded album-card-big-border`}>
                <img className={`card-img-top ${this.props.isBiggerCard ? 'genre-card-img-big' : 'genre-card-img-small'}`} src={imageSrc} alt="Card image cap" />
                <div className="card-body bgblack">
                    <h5 className={`cardtitle ${this.props.isBiggerCard ? '' : 'genre-card-small-font'}`}>{this.props.title}</h5>
                </div>
                <div className={`album-card-big-likes ${this.props.isBiggerCard ? '' : 'album-card-big-likes-small'}`}>{this.props.likesCount} likes</div>
            </Link>
        );
    }
}

export default AlbumCardBig;