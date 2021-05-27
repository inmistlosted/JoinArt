import React, {Component} from 'react';
import '../componentsStyles/ConfirmPopup.css';

class ConfirmPopup extends Component{
    render() {
        return(
            <div className={'confirm-popup-container'} onClick={this.props.closePopup}>
                <div className={'confirm-popup'} onClick={this.props.keepPopup}>
                    <div className={'confirm-popup-question'}>{this.props.question}</div>
                    <div className={'confirm-popup-btns'}>
                        <div className={'confirm-popup-yes'} onClick={this.props.confirm}>Yes</div>
                        <div className={'confirm-popup-no'} onClick={this.props.decline}>No</div>
                    </div>
                </div>
            </div>
        );
    }
}

export default ConfirmPopup;