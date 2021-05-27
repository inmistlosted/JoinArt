import React, {Component} from 'react'

import 'bootstrap/dist/css/bootstrap.css'
import "../componentsStyles/GenreCardStyle.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

class BidCard extends Component{

    render() {
        const imageSrc = this.props.image ? this.props.image : process.env.PUBLIC_URL + '/pic.jpg';

        return (
            <Link to={`auction/${this.props.id}`} className={`card, cardsize rounded ${this.props.status ? 'bid-card-border-active' : 'bid-card-border-ended'}`}>
                <img className={`card-img-top ${this.props.isBiggerCard ? 'genre-card-img-big' : 'genre-card-img-small'}`} src={imageSrc} alt="Card image cap" />
                <div className="card-body bgblack">
                    <h5 className={`cardtitle ${this.props.isBiggerCard ? '' : 'genre-card-small-font'}`}>Auction #{this.props.id}</h5>
                    <h5 className={`bid-card-price ${this.props.isBiggerCard ? '' : 'genre-card-small-font'}`}>{this.props.status ? `$${this.props.price}` : 'Sold'}</h5>
                </div>
            </Link>
        );
    }
}

export default BidCard;