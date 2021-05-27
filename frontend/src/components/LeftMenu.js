import React, {Component} from 'react';
import '../componentsStyles/LeftMenu.css';
import {Link} from 'react-router-dom';

class LeftMenu extends Component{
    constructor(props) {
        super(props);
    }

    render() {
        let currHelpClass = this.props.wasMenuClicked ? 'left-menu-full-hidden' : '';
        const home = 'Home';
        const genres = 'Genres';
        const albums = 'Albums';
        const auctions = 'Auctions';

        return(
            <div className={`left-menu-full ${this.props.isMenuClicked ? 'left-menu-full-active' : currHelpClass}`}>
                <div className={'left-menu-line'}/>
                <Link to={'/'} className={`left-menu-bars-full ${this.props.currentPageTitle === home ? 'active' : ''}`} onClick={() => this.props.changePageTitle(home)}>
                    <div className={'bar-title'}>Home</div>
                    <div className={'bar-icon'}>
                        <i className="fa fa-home" />
                    </div>
                </Link>
                <Link to={'/genres'} className={`left-menu-bars-full ${this.props.currentPageTitle === genres ? 'active' : ''}`} onClick={() => this.props.changePageTitle(genres)}>
                    <div className={'bar-title'}>Genres</div>
                    <div className={'bar-icon'}>
                        <i className="fa fa-pencil" />
                    </div>
                </Link>
                <Link to={'/albums'} className={`left-menu-bars-full ${this.props.currentPageTitle === albums ? 'active' : ''}`} onClick={() => this.props.changePageTitle(albums)}>
                    <div className={'bar-title'}>Albums</div>
                    <div className={'bar-icon'}>
                        <i className="fa fa-image" />
                    </div>
                </Link>
                <Link to={'/auctions'} className={`left-menu-bars-full ${this.props.currentPageTitle === auctions ? 'active' : ''}`} onClick={() => this.props.changePageTitle(auctions)}>
                    <div className={'bar-title'}>Auctions</div>
                    <div className={'bar-icon'}>
                        <i className="fa fa-money" />
                    </div>
                </Link>
            </div>
        );
    }
}

export default LeftMenu;