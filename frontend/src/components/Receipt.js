import React, {Component} from 'react';
import '../componentsStyles/Receipt.css';

class Receipt extends Component{
    render() {
        return(
            <div className={'receipt-container'} onClick={this.props.closeReceipt}>
                <div className={'receipt'} onClick={this.props.keepReceipt}>
                    <div className={'receipt-title'}>Receipt</div>
                    <div className={'receipt-info'}>
                        <div className={'receipt-painting-title'}>{this.props.itemTitle}</div>
                        <div className={'receipt-count'}>x {this.props.itemCount}</div>
                        <div className={'receipt-painting-price'}>${this.props.itemPrice}</div>
                    </div>
                    <div className={'receipt-total'}>
                        <div className={'receipt-total-title'}>Total</div>
                        <div className={'receipt-total-price'}>${this.props.amount}</div>
                    </div>
                    <div className={'receipt-prediction'}>
                        <div className={'receipt-prediction-title'}>Prediction for you:
                            <span className={'receipt-prediction-content'}>today is your lucky day</span>
                        </div>
                    </div>
                    <div className={'receipt-submit-btn'} onClick={() => this.props.submitOrder(this.props.orderId)}>Submit</div>
                </div>
            </div>
        );
    }
}

export default Receipt;