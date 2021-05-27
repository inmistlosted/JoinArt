import React, {Component} from 'react';
import '../componentsStyles/Menu.css';
import UpperToolBar from './UpperToolBar';
import LeftMenu from './LeftMenu';

class Menu extends Component{
    constructor(props) {
        super(props);

        this.state = {
            leftMenuClicked: false,
            leftMenuWasClicked: false
        };

        this.clickLeftMenu = this.clickLeftMenu.bind(this);
    }

    clickLeftMenu(){
        this.setState({
            leftMenuClicked: !this.state.leftMenuClicked,
            leftMenuWasClicked: true
        })
    }

    render() {
        return(
            <div className={'menu'}>
                <UpperToolBar
                    clickLeftMenu={this.clickLeftMenu}
                    isMenuClicked={this.state.leftMenuClicked}
                    changePageTitle={this.props.changePageTitle}
                    userLogin={this.props.userLogin}
                    logout={this.props.logout}
                />
                <LeftMenu
                    isMenuClicked={this.state.leftMenuClicked}
                    wasMenuClicked={this.state.leftMenuWasClicked}
                    changePageTitle={this.props.changePageTitle}
                    currentPageTitle={this.props.currentPageTitle} />
                   <div className={'menu-page-title'}>
                    {this.props.currentPageTitle}
                </div>
                <div className={`full-menu-shadow ${this.state.leftMenuClicked ? 'active' : ''}`} />
            </div>
        );
    }
}

export default Menu;