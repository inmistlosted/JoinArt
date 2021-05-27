import React, {Component} from 'react';
import '../componentsStyles/PlaceBetForm.css';

class PlaceBetForm extends Component{
    constructor(props) {
        super(props);

        this.state = {
            content: ''
        };
    }

    render() {
        return(
            <div className={'bet-form-container'} onClick={this.props.closePopup}>
                <div className={'bet-form'} onClick={this.props.keepPopup}>
                    <div className={'bet-form-title'}>
                        Auction #34
                    </div>
                    <input type={'text'} className={`add-comment-content ${this.props.betError ? 'bet-form-error' : ''}`} placeholder={`min $${this.props.minPrice+1}`} value={this.props.bet} onInput={this.props.handleInput}/>
                    <div className={'bet-form-btn'} onClick={this.props.placeBet}>Place bet</div>
                </div>
            </div>
        );
    }
}

export default PlaceBetForm;