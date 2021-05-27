import React, {Component} from 'react'
import "../componentsStyles/Albums.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

import {Loading} from './Loading';
import AuctionService from '../services/AuctionService';
import BidCard from './BidCard';

class Auctions extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0,0);

        this.state = {
            allAuctions: [],
            topAuctions: [],
            isDataLoaded: false
        }
    }

    async componentDidMount() {
        const allAuctions = await AuctionService.getAllAuctions();
        const topAuction = await AuctionService.getTopAuctions();

        this.setState({
            allAuctions: allAuctions,
            topAuctions: topAuction,
            isDataLoaded: true
        });
    }

    render() {
        const topAuctions = this.state.topAuctions.map((auction) => {
            return(
                <div key={auction.bidId} className="col-md-2">
                    <BidCard id={auction.bidId} image={auction.paintingImage} status={auction.status} price={auction.currentPrice} isBiggerCard={false} />
                </div>
            );
        });

        const allAuctions = this.state.topAuctions.map((auction) => {
            return(
                <div key={auction.bidId} className="col-md-3">
                    <BidCard id={auction.bidId} image={auction.paintingImage} status={auction.status} price={auction.currentPrice} isBiggerCard={true} />
                </div>
            );
        });

        if(this.state.isDataLoaded){
            return (
                <div className="container">
                    <div className="row ">
                        <div className="col-md-12 toptopicstitle">
                            <h4>Top auctions</h4>
                        </div>
                    </div>
                    <div className="row ">
                        {topAuctions}
                    </div>
                    <div className="row ">
                        <div className="col-md-12 alltopicstitle">
                            <h4>All auctions</h4>
                        </div>
                        {allAuctions}
                    </div>
                </div>
            );
        }else{
            return <Loading/>
        }
    }
}


export default Auctions;