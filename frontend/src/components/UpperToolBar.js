import React, {Component} from 'react'
import "../componentsStyles/UpperToolBar.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter
} from "react-router-dom";

class UpperToolBar extends Component{
    constructor(props) {
        super(props);

        this.state = {
            searchQuery: ''
        };
    }

    search = () => {
        if (this.state.searchQuery.length > 0){
            window.location.assign(`/search?query=${this.state.searchQuery}`);

            this.setState({searchQuery: ''})
        }
    }

    searchByEnter = (e) => {
        if(e.keyCode === 13){
            this.search();
        }
    }

    handleSearchQueryChange = (e) => {
        this.setState({searchQuery: e.target.value});
    }

    render() {
        const loginBtns = (
            <div className={'upper-tool-bar-right'}>
                <Link to={'/login'} className="upper-tool-bar-right-btn" onClick={() => this.props.changePageTitle('Login')}>
                    <a aria-current="page" href="#">Login</a>
                </Link>
                <Link to={'/signup'} className="upper-tool-bar-right-btn" onClick={() => this.props.changePageTitle('Sign up')}>
                    <a href="#">Sign up</a>
                </Link>
            </div>
        );

        const userBtns = (
            <div className={'upper-tool-bar-right'}>
                <Link to={'/user'} className="upper-tool-bar-right-btn logged-in" onClick={() => this.props.changePageTitle(this.props.userLogin)}>
                    <div aria-current="page">{this.props.userLogin}</div>
                </Link>
                <div className="upper-tool-bar-right-btn" onClick={this.props.logout}>
                    <div>Sign out</div>
                </div>
            </div>
        );

        return (
            <nav className={'upper-tool-bar'}>
                <div className={'upper-tool-bar-left'}>
                    <div className={`menu-bar ${this.props.isMenuClicked ? 'active' : ''}`} onClick={this.props.clickLeftMenu}>
                        <i className="fa fa-bars" />
                    </div>
                    <div className={'search-block'}>
                        <div className={'title-text'}>
                            <Link to={'../'} onClick={() => this.props.changePageTitle('Home')}>JoinArt</Link>
                        </div>
                        <div className={'upper-tool-bar-line'}/>
                        <div className={'search-input'}>
                            <div className={'search-input-container'}>
                                <i className={'fa fa-search search-btn'} onClick={this.search}/>
                                <input type="text" placeholder="Search" aria-label="Search" value={this.state.searchQuery} onChange={this.handleSearchQueryChange} onKeyDown={this.searchByEnter}/>
                            </div>
                        </div>
                    </div>
                </div>
                {this.props.userLogin.length > 0 ? userBtns : loginBtns}
            </nav>
        );
    }
}

export default withRouter(UpperToolBar);