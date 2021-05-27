import React, {Component} from 'react';
import Cookies from 'universal-cookie/lib';
import logo from './logo.svg';
import './App.css';
import UpperToolBar from './components/UpperToolBar'
import Genres from './components/Genres'
import Login from './components/Login'
import Footer from './components/Footer'
import Menu from './components/Menu';
import SignUp from './components/SignUp';
import PaintingPage from './components/PaintingPage';
import {
  BrowserRouter as Router,
  Switch,
  Route,
    useLocation
} from 'react-router-dom';
import Home from './components/Home';
import AlbomPage from './components/AlbomPage';
import GenrePage from './components/GenrePage';
import Albums from './components/Albums';
import AlbumPage from './components/AlbumPage';
import SearchPage from './components/SearchPage';
import AuctionPage from './components/AuctionPage';
import Auctions from './components/Auctions';
import UserPage from './components/UserPage';
import ManagePaintings from './components/ManagePaintings';
import Purchases from './components/Purchases';
import UserPaintingsPage from './components/UserPaintingsPage';
import Analytics from './components/Analytics';
import Admin from './components/Admin';


class App extends Component{
    constructor(props) {
        super(props);

        const cookies = new Cookies();
        const userLogin = cookies.get('login');
        const userId = cookies.get('userId');
        const pattern = /\/.*(?=\/)|\/.*/g;

        const routesPagesDict = {
            "/":"Home",
            "/genres" : "Genres",
            "/genre": "Genres",
            "/login" : "Login",
            "/signup" : "Sign up",
            "/albums" : "Albums",
            "/album" : "Albums",
            "/search": "Search",
            "/auctions": "Auctions",
            "/auction": "Auctions",
            "/user": userLogin ? userLogin : "",
            "/user-paintings": "Paintings",
            "/user/analytics": "Analytics",
            "/admin": "Administrative panel"
        };

        this.state = {
            currentPageTitle: routesPagesDict[pattern.exec(window.location.pathname)[0]],
            userLogin: userLogin ? userLogin : '',
            userId: userId ? userId : 0
        };

        this.changePageTitle = this.changePageTitle.bind(this);
    }

    changePageTitle(pageTitle){
        this.setState({
            currentPageTitle: pageTitle
        });
    }

    authorizeUser = (login, userId, roleId, saveInCookies) => {
        this.setState({
            userLogin: login,
            userId: userId
        });

        if(saveInCookies){
            const cookies = new Cookies();
            cookies.set('login', login, { path: '/' });
            cookies.set('userId', userId, { path: '/' });
            cookies.set('roleId', roleId, { path: '/' });
        }
    }

    logoutUser = () => {
        this.setState({
            userLogin: '',
            userId: 0
        });

        const cookies = new Cookies();
        cookies.remove('login');
        cookies.remove('roleId');
        cookies.remove('userId');
    }

    render() {
        return (
            <Router>
                <Menu changePageTitle={this.changePageTitle} userLogin={this.state.userLogin} currentPageTitle={this.state.currentPageTitle} logout={this.logoutUser}/>
                <div className={'content-container'}>
                    <div className={'content '}>
                        <Switch>
                            <Route exact path="/" component={() => <Home userId={this.state.userId} changePageTitle={this.changePageTitle}/>} />
                            <Route exact path="/genres" component={() => <Genres />} />
                            <Route exact path="/login" component={() => <Login authorizeUser={this.authorizeUser} userLogin={this.state.userLogin}/>} />
                            <Route exact path="/signup" component={() => <SignUp authorizeUser={this.authorizeUser} userLogin={this.state.userLogin}/>} />
                            <Route exact path="/painting/:id" component={(props) => <PaintingPage userId={this.state.userId} userLogin={this.state.userLogin} {...props}/>} />
                            <Route exact path="/genre/:id" component={(props) => <GenrePage userId={this.state.userId} {...props} changePageTitle={this.changePageTitle}/>} />
                            <Route exact path="/albums" component={() => <Albums />} />
                            <Route exact path="/album/:id" component={(props) => <AlbumPage userId={this.state.userId} {...props} changePageTitle={this.changePageTitle}/>} />
                            <Route exact path="/search" component={(props) => <SearchPage userId={this.state.userId} {...props} changePageTitle={this.changePageTitle}/>} />
                            <Route exact path="/auctions" component={() => <Auctions />} />
                            <Route exact path="/auction/:id" component={(props) => <AuctionPage {...props} userId={this.state.userId} userLogin={this.state.userLogin}/>} />
                            <Route exact path="/user" component={(props) => <UserPage {...props} userId={this.state.userId} userLogin={this.state.userLogin} authorizeUser={this.authorizeUser} changePageTitle={this.changePageTitle}/>} />
                            <Route exact path="/user/manage-paintings" component={(props) => <ManagePaintings {...props} userId={this.state.userId} userLogin={this.state.userLogin}/>} />
                            <Route exact path="/user/purchases" component={(props) => <Purchases {...props} userId={this.state.userId} userLogin={this.state.userLogin}/>} />
                            <Route exact path="/user-paintings/:userId" component={(props) => <UserPaintingsPage {...props} userId={this.state.userId} changePageTitle={this.changePageTitle} />} />
                            <Route exact path="/user/analytics" component={(props) => <Analytics {...props} userId={this.state.userId} userLogin={this.state.userLogin}/>} />
                            <Route exact path="/admin" component={(props) => <Admin {...props} userId={this.state.userId} userLogin={this.state.userLogin}/>} />
                        </Switch>
                        <Footer />
                    </div>
                </div>

            </Router>
        );
    }
}

export default App;
