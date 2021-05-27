import React, {Component} from 'react';
import '../componentsStyles/InfoPopup.css';

class InfoPopup extends Component{
    render() {
        return(
            <div className={'info-popup-container'} onClick={this.props.closePopup}>
                <div className={'info-popup'}>
                    {this.props.message}
                </div>
            </div>
        );
    }
}

export default InfoPopup;





