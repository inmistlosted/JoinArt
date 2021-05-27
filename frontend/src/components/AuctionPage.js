import React, {Component} from 'react';
import '../componentsStyles/AuctionPage.css';
import PaintingService from '../services/PaintingService';
import AuctionService from '../services/AuctionService';
import {Loading} from './Loading';
import PlaceBetForm from './PlaceBetForm';
import InfoPopup from './InfoPopup';
import Comment from './Comment';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    Redirect
} from "react-router-dom";

class AuctionPage extends Component{
    constructor(props) {
        super(props);
        this.bidTimer = 0;

        this.state = {
            bid: null,
            isDataLoaded: false,
            isBetFormOpened: false,
            status: false,
            hasStarted: false,
            bet: '',
            showPopup: false,
            popupMessage: '',
            price: 0,
            history: []
        };
    }

    async componentDidMount() {
        const id = this.props.match.params.id;

        const bid = await AuctionService.getBid(id);
        let timerSeconds = 0;

        if(bid.hasStarted){
            timerSeconds = Math.abs(new Date(bid.endTime) - new Date()) / 1000;
            this.bidTimer = setInterval(this.countDown, 1000);
        }

        this.setState({
            bid: bid,
            timerSeconds: timerSeconds,
            timer: this.secondsToTime(timerSeconds),
            status: bid.status,
            hasStarted: bid.hasStarted,
            price: bid.currentPrice,
            history: bid.history,
            isDataLoaded: true
        });
    }

    countDown = () => {
        let seconds = this.state.timerSeconds - 1;
        this.setState({
            timer: this.secondsToTime(seconds),
            timerSeconds: seconds,
        });

        if (seconds === 0) {
            clearInterval(this.bidTimer);
        }
    }

    secondsToTime(secs){
        let hours = Math.floor(secs / (60 * 60));

        let divisor_for_minutes = secs % (60 * 60);
        let minutes = Math.floor(divisor_for_minutes / 60);

        let divisor_for_seconds = divisor_for_minutes % 60;
        let seconds = Math.ceil(divisor_for_seconds);

        let obj = {
            "h": hours < 10 ? `0${hours}` : hours,
            "m": minutes < 10 ? `0${minutes}` : minutes,
            "s": seconds < 10 ? `0${seconds}` : seconds
        };
        return obj;
    }

    openBetForm = () => {
        this.setState({isBetFormOpened: true});
    }

    closeBetForm = () => {
        this.setState({isBetFormOpened: false});
    }

    keepBetForm = (e) => {
        e.stopPropagation();
    }

    handleBetInput = (e) => {
        const oldValue = this.state.bet;

        if(e.target.value.length === 0){
            this.setState({bet: ''});
        }else if(isNaN(e.target.value)){
            this.setState({bet: oldValue});
        }else{
            this.setState({bet: +e.target.value, betError: false});
        }
    }

    closePopup = () => {
        this.setState({showPopup: false});
    }

    startBid = async () => {
        const response = await AuctionService.startBid(this.state.bid.bidId, this.props.userId);

        if(response.status){
            const startTime = new Date();
            const endTime = new Date();
            endTime.setDate(startTime.getDate() + 1);
            const timerSeconds = Math.abs(endTime - startTime) / 1000;
            this.bidTimer = setInterval(this.countDown, 1000);

            this.setState({
                hasStarted: true,
                timerSeconds: timerSeconds,
                timer: this.secondsToTime(timerSeconds),
                history: [
                    {
                        betId: 0,
                        userLogin: this.props.userLogin,
                        bet: this.state.price,
                        placeBetTime: (new Date()).toString()
                    },
                    ...this.state.history
                ]
            });
        }else{
            this.setState({showPopup: true, popupMessage: response.message});
        }
    }

    placeBet = async () => {
        if(this.state.bet.length === 0){
            this.setState({betError: true});
        }else if(this.state.bet <= this.state.price){
            this.setState({showPopup: true, popupMessage: 'Your bet must be higher than the current price'});
        }else{
            const response = await AuctionService.placeBet(this.state.bid.bidId, this.props.userId, this.state.bet);

            if(response.status){
                this.setState({
                    isBetFormOpened: false,
                    price: this.state.bet,
                    bet: '',
                    history: [
                        {
                            betId: this.state.history[this.state.history.length-1].betId,
                            userLogin: this.props.userLogin,
                            bet: this.state.bet,
                            placeBetTime: (new Date()).toString()
                        },
                        ...this.state.history
                    ]
                });
            }else{
                this.setState({showPopup: true, popupMessage: response.message});
            }
        }
    }

    render() {
        if(this.props.userId !== 0){
            if(this.state.isDataLoaded){
                if(this.state.bid != null){
                    let timer = <span className={'auction-page-ended'}>Ended</span>;

                    if(this.state.status){
                        timer = this.state.hasStarted
                            ? `${this.state.timer.h}:${this.state.timer.m}:${this.state.timer.s}`
                            : <span className={'auction-page-not-started'}>Has not started yet</span>;
                    }

                    const bidBtn = this.state.hasStarted
                        ? <div className={'auction-page-bet-btn'} onClick={this.openBetForm}>Place bet</div>
                        : <div className={'auction-page-bet-btn'} onClick={this.startBid}>Start auction</div>;

                    const auctionBtn = this.state.status ? bidBtn : '';

                    const betForm = this.state.isBetFormOpened
                        ? <PlaceBetForm
                            closePopup={this.closeBetForm}
                            keepPopup={this.keepBetForm}
                            handleInput={this.handleBetInput}
                            bet={this.state.bet}
                            betError={this.state.betError}
                            placeBet={this.placeBet}
                            bidId={this.state.bid.bidId}
                            minPrice={this.state.price}/> : '';

                    const popup = this.state.showPopup ? <InfoPopup message={this.state.popupMessage} closePopup={this.closePopup}/> : '';

                    const history = this.state.history.length > 0 ? this.state.history.map((item, index) => {
                        return(
                            <div key={item.betId} className={'auction-page-history-bet'}>
                                <div className={'auction-page-history-bet-title'}>
                                    <span className={'auction-page-history-bet-line'}>--</span>
                                    {item.userLogin}
                                    <span className={'auction-page-history-bet-price'}>${item.bet}</span>
                                    {index === 0 ? <span className={`auction-page-history-bet-mark ${this.state.status ? 'bet-active' : 'bet-ended'}`}>{this.state.status ? 'Top bet' : 'Buyer'}</span> : ''}
                                </div>
                                <div className={'auction-page-history-bet-time'}>{(new Date(item.placeBetTime)).toLocaleString()}</div>
                            </div>
                        );
                    }) : <div className={'no-comments-found'}>No history found</div>;

                    return(
                        <div className={'auction-page-container'}>
                            {betForm}
                            {popup}
                            <div className={'auction-page-title-container'}>
                                <div className={'auction-page-title'}>Auction #{this.state.bid.bidId}</div>
                                <div className={'auction-page-time-title'}>
                                    {timer}
                                </div>
                            </div>
                            <div className={'auction-page-info-container'}>
                                <div className={'auction-page-bid-info'}>
                                    <div className={'auction-page-bid-title'}>Bid info</div>
                                    <div className={'auction-page-painting'}>
                                        <img className={'auction-page-painting-image'} src={this.state.bid.paintingImage} />
                                        <div className={'auction-page-painting-title'}>{this.state.bid.paintingTitle}</div>
                                    </div>
                                </div>
                                <div className={'auction-page-info'}>
                                    <div className={'auction-page-price-container'}>
                                        <div className={'auction-page-price-title'}>{this.state.status ? `$${this.state.price}` : 'Sold'}</div>
                                        {auctionBtn}
                                    </div>
                                    <div className={'auction-page-history-container'}>
                                        <div className={'auction-page-history-title'}>Bet history</div>
                                        <div className={'auction-page-history'}>
                                            {history}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    );
                }else{
                    return <div>Bid not found</div>
                }
            }else{
                return <Loading />
            }
        }else{
            return <Redirect to={'/login'} />;
        }
    }
}

export default AuctionPage;