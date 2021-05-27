import React, {Component} from 'react';
import '../componentsStyles/SearchPage.css';
import GenreService from '../services/GenreService';
import {Loading} from './Loading';
import Painting from './paintings/Painting';
import AlbumService from '../services/AlbumService';
import PaintingService from '../services/PaintingService';
import UserService from '../services/UserService';
import InfoPopup from './InfoPopup';

class UserPaintingsPage extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0, 0);

        this.state = {
            paintings: [],
            pageUserId: 0,
            isDataLoaded: false
        };
    }

    async componentDidMount() {
        const userId = this.props.match.params.userId;
        const userInfo = await UserService.getUserInfo(userId, this.props.userId);
        const paintings = await PaintingService.getUserPaintings(userId, this.props.userId);

        this.setState({
            paintings: paintings,
            userLogin: userInfo.login,
            isFollowing: userInfo.isFollowing,
            pageUserId: +userId,
            isDataLoaded: true,
            showPopup: false,
            popupMessage: ''
        });
    }

    handleFollowChange = async () => {
        if(this.props.userId === 0){
            window.scrollTo(0, 0);
            this.setState({showPopup: true, popupMessage: 'Please login first'});
        }else{
            if(this.state.isFollowing){
                await UserService.unfollowUser(this.props.userId, this.state.pageUserId);
            }else{
                await UserService.followUser(this.props.userId, this.state.pageUserId);
            }

            this.setState({isFollowing: !this.state.isFollowing});
        }
    }

    closePopup = () => {
        this.setState({showPopup: false});
    }

    render() {
        if(this.state.isDataLoaded){
            const paintings = this.state.paintings != null && this.state.paintings.length > 0 ?
                this.state.paintings.map((painting) => {
                    return(<Painting key={painting.paintingId}
                                     id={painting.paintingId}
                                     image={painting.imagePath}
                                     title={painting.title}
                                     author={painting.painter}
                                     status={painting.status}
                                     price={painting.price}
                                     likesCount={painting.likesCount}
                                     isLiked={painting.isLiked}
                                     commentsCount={painting.commentsCount}
                                     userId={this.props.userId}
                                     changePageTitle={this.props.changePageTitle}/>);
                }) : <div className={'genre-no-paintings-found'}>No paintings found</div>;

            const followBtn = this.state.pageUserId !== +this.props.userId
                ? <div className={`user-paints-page-follow-btn ${this.state.isFollowing ? 'user-paints-page-following' : ''}`} onClick={this.handleFollowChange}>{this.state.isFollowing ? 'Following' : 'Follow'}</div>
                : '';

            const popup = this.state.showPopup ? <InfoPopup message={this.state.popupMessage} closePopup={this.closePopup}/> : '';

            return(
                <div className={'search-page-container'}>
                    {popup}
                    <div className={'search-page-query-title user-paints-page-title'}>
                        <div>Paintings of user <span className={'search-page-query'}>{this.state.userLogin}</span></div>
                        {followBtn}
                    </div>
                    <div className={'search-page-paintings'}>
                        {paintings}
                    </div>
                </div>
            );
        }else{
            return <Loading/>;
        }
    }
}

export default UserPaintingsPage;