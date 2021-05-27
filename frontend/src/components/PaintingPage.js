import React, {Component} from 'react'
import "../componentsStyles/PaintingPage.css"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    useParams
} from "react-router-dom";
import Comment from "./Comment";
import PaintingService from '../services/PaintingService';
import GenreCard from './GenreCard';
import {Loading} from './Loading';
import InfoPopup from './InfoPopup';
import AddComment from './AddComment';
import UserService from '../services/UserService';
import AddToAlbum from './AddToAlbum';
import CreateAlbum from './CreateAlbum';
import ConfirmPopup from './ConfirmPopup';
import AlbumService from '../services/AlbumService';
import AuctionService from '../services/AuctionService';

class PaintingPage extends Component{
    constructor(props) {
        super(props);
        window.scrollTo(0, 0);

        this.state = {
            showAuthor: false,
            showMaterial: false,
            showDescription: false,
            isDataLoaded: false,
            isLiked: false,
            showPopup: false,
            showAddCommentPopup: false,
            showAddToAlbumPopup: false,
            showCreateAlbumPopup: false,
            showConfirmPopup: false,
            isFollowing: false,
            popupMessage: '',
            commentContent: '',
            likesCount: 0,
            commentsCount: 0,
            comments: [],
            newAlbumTitle: '',
            newAlbumDescription: '',
            newAlbumError: false,
            currentAlbumId: 0,
            currentAlbumTitle: '',
            addToAlbumQuestion: '',


            painting: null
        };
    }

    async componentDidMount() {
        const id = this.props.match.params.id;

        const painting = await PaintingService.getPainting(id, this.props.userId);

        this.setState({
            painting: painting,
            isLiked: painting.isLiked,
            likesCount: painting.likesCount,
            commentsCount: painting.commentsCount,
            comments: painting.comments,
            isFollowing: painting.isFollowingOwner,
            isDataLoaded: true
        });
    }

    handleAuthorChange = () => {
        this.setState({showAuthor: !this.state.showAuthor});
    }

    handleMaterialsChange = () => {
        this.setState({showMaterial: !this.state.showMaterial});
    }

    handleDescrChange = () => {
        this.setState({showDescription: !this.state.showDescription});
    }

    closePopup = () => {
        this.setState({showPopup: false});
    }

    closeAddCommentPopup = () => {
        this.setState({showAddCommentPopup: false});
    }

    keepAddCommentPopup = (e) => {
        e.stopPropagation();
        this.setState({showAddCommentPopup: true});
    }

    closeAddToAlbumPopup = () => {
        this.setState({showAddToAlbumPopup: false});
    }

    keepAddToAlbumPopup = (e) => {
        e.stopPropagation();
        this.setState({showAddToAlbumPopup: true});
    }

    closeCreateAlbumPopup = () => {
        this.setState({showCreateAlbumPopup: false});
    }

    keepCreateAlbumPopup = (e) => {
        e.stopPropagation();
        this.setState({showCreateAlbumPopup: true});
    }

    closeConfirmPopup = () => {
        this.setState({showConfirmPopup: false});
    }

    keepConfirmPopup = (e) => {
        e.stopPropagation();
    }

    handleLike = () => {
        if(this.props.userId === 0){
            window.scrollTo(0, 0);
            this.setState({showPopup: true, popupMessage: 'Please login first'});
        }else{
            if(this.state.isLiked){
                PaintingService.removeLike(this.state.painting.paintingId, this.props.userId);
                this.setState({isLiked: false, likesCount: this.state.likesCount-1});
            }else{
                PaintingService.addLike(this.state.painting.paintingId, this.props.userId);
                this.setState({isLiked: true, likesCount: this.state.likesCount+1});
            }
        }
    }

    openCommentFrom = () => {
        if(this.props.userId === 0){
            window.scrollTo(0, 0);
            this.setState({showPopup: true, popupMessage: 'Please login first'});
        }else{
            window.scrollTo(0, 0);
            this.setState({showAddCommentPopup: true});
        }
    }

    writeComment = (e) =>{
        this.setState({commentContent: e.target.value});
    }

    addComment = async () => {
        if(this.state.commentContent.length > 0){
            await PaintingService.addComment(this.state.painting.paintingId, this.props.userId, this.state.commentContent);
            this.setState({showAddCommentPopup: false, comments: [...this.state.comments, {
                id: this.props.userId, content: this.state.commentContent, userName: this.props.userLogin, date: new Date()
            }]});
        }
    }

    handleFollowChange = async () => {
        if(this.props.userId === 0){
            window.scrollTo(0, 0);
            this.setState({showPopup: true, popupMessage: 'Please login first'});
        }else{
            if(this.state.isFollowing){
                await UserService.unfollowUser(this.props.userId, this.state.painting.owner.userId);
            }else{
                await UserService.followUser(this.props.userId, this.state.painting.owner.userId);
            }

            this.setState({isFollowing: !this.state.isFollowing});
        }
    }

    openAddToAlbumPopup = () => {
        if(this.props.userId === 0){
            window.scrollTo(0, 0);
            this.setState({showPopup: true, popupMessage: 'Please login first'});
        }else{
            window.scrollTo(0, 0);
            this.setState({showAddToAlbumPopup: true});
        }
    }

    openCreateAlbumPopup = () => {
        window.scrollTo(0, 0);
        this.setState({showCreateAlbumPopup: true});
    }

    handleNewAlbumTitleChange = (e) => {
        this.setState({newAlbumTitle: e.target.value});
    }

    handleNewAlbumDescChange = (e) => {
        this.setState({newAlbumDescription: e.target.value});
    }

    openConfirmPopup = (albumId, albumTitle) => {
        window.scrollTo(0, 0);
        this.setState({
            showConfirmPopup: true,
            currentAlbumId: albumId,
            addToAlbumQuestion: <div>Do you want to add painting <span className={'confirm-popup-painting-title'}>{this.state.painting.title}</span> to album <span className={'confirm-popup-album-title'}>{albumTitle}</span>?</div>
        });
    }

    createAlbum = async () => {
        if(this.state.newAlbumTitle.length === 0){
            this.setState({newAlbumError: true});
        }else{
            const albumResponse = await AlbumService.create(this.state.newAlbumTitle, this.state.newAlbumDescription, this.props.userId);

            this.setState({
                newAlbumError: false,
                showCreateAlbumPopup: false,
                showAddToAlbumPopup: false,
                newAlbumTitle: '',
                newAlbumDescription: '',
                showConfirmPopup: true,
                currentAlbumId: albumResponse.albumId,
                addToAlbumQuestion: <div>Do you want to add painting <span className={'confirm-popup-painting-title'}>{this.state.painting.title}</span> to album <span className={'confirm-popup-album-title'}>{this.state.newAlbumTitle}</span>?</div>
            });
        }
    }

    addToAlbum = async () => {
        if(this.state.currentAlbumId > 0 && this.state.painting.paintingId > 0){
            const response = await AlbumService.addToAlbum(this.state.currentAlbumId, this.state.painting.paintingId);

            if(!response.isAdded){
               this.setState({
                   showPopup: true,
                   popupMessage: response.message
               });
            }

            this.setState({
                showAddToAlbumPopup: false,
                showCreateAlbumPopup: false,
                showConfirmPopup: false,
                currentAlbumId: 0,
                addToAlbumQuestion: ''
            });
        }
    }

    declineAddToAlbum = () => {
        this.setState({
            showConfirmPopup: false,
            showCreateAlbumPopup: false,
            currentAlbumId: 0,
            addToAlbumQuestion: ''
        }, () => {
            this.setState({
                showAddToAlbumPopup: true
            });
        });
    }

    handleAuctionBtn = async () =>{
        if(this.props.userId === 0){
            window.scrollTo(0, 0);
            this.setState({showPopup: true, popupMessage: 'Please login first'});
        }else{
            const response = await AuctionService.getPaintingBidId(this.state.painting.paintingId);

            if(response.status){
                window.location.assign(`/auction/${response.bidId}`);
            }else{
                this.setState({showPopup: true, popupMessage: response.message});
            }
        }
    }

    render() {
        if(this.state.isDataLoaded){
            if(this.state.painting != null){
                const materials = this.state.painting.materials ?
                    <div className={'art-page-author-container'}>
                        <div className={'art-page-author-title'} onClick={this.handleMaterialsChange}>
                            <div>Art materials</div>
                            {this.state.showMaterial ? <i className={'fa fa-chevron-up'} /> : <i className={'fa fa-chevron-down'} />}
                        </div>
                        <div className={`art-page-author-name ${this.state.showMaterial ? 'active' : ''}`}>{this.state.painting.materials}</div>
                    </div> : '';

                const description = this.state.painting.description ?
                    <div className={'art-page-author-container'}>
                        <div className={'art-page-author-title'} onClick={this.handleDescrChange}>
                            <div>Description</div>
                            {this.state.showDescription ? <i className={'fa fa-chevron-up'} /> : <i className={'fa fa-chevron-down'} />}
                        </div>
                        <div className={`art-page-author-name ${this.state.showDescription ? 'active' : ''}`}>{this.state.painting.description}</div>
                    </div> : '';

                const comments = this.state.comments.length > 0 ? this.state.comments.map((comment) => {
                    return(<Comment key={comment.id} id={comment.id} content={comment.content} username={comment.userName} date={comment.date}/>);
                }) : <div className={'no-comments-found'}>No comments found</div>;

                const auctionBtn = this.state.painting.status ?
                    <div className={'art-page-auction-btn'} onClick={this.handleAuctionBtn}>{this.state.painting.isBidProduct ? 'Join auction' : 'Start auction'}</div>
                    : '';

                const followBtn = this.state.painting.owner && +this.state.painting.owner.userId !== +this.props.userId
                    ? <div className={`art-page-follow-btn ${this.state.isFollowing ? 'art-page-is-following' : ''}`} onClick={this.handleFollowChange}>{this.state.isFollowing ? 'Following' : 'Follow'}</div>
                    : '';

                const owner = this.state.painting.owner ?
                    <div className={'art-page-owner-container'}>
                        <div className={'art-page-owner'}>Owner: <Link to={`/user-paintings/${this.state.painting.owner.userId}`} className={'art-page-owner-name'}>{this.state.painting.owner.login}</Link></div>
                        {followBtn}
                    </div> : '';

                const genres = this.state.painting.genres.map((genre) => {
                    return(<Link to={`../genre/${genre.genreId}`} key={genre.genreId} className={`art-page-genre ${genre.isMovement ? 'art-page-movement' : ''}`}>{genre.title}</Link>);
                });

                const popup = this.state.showPopup ? <InfoPopup message={this.state.popupMessage} closePopup={this.closePopup}/> : '';
                const addCommentForm = this.state.showAddCommentPopup
                    ? <AddComment
                        closePopup={this.closeAddCommentPopup}
                        keepPopup={this.keepAddCommentPopup}
                        content={this.state.commentContent}
                        writeComment={this.writeComment}
                        addComment={this.addComment}/> : '';
                const addToAlbum = this.state.showAddToAlbumPopup
                    ? <AddToAlbum
                        closePopup={this.closeAddToAlbumPopup}
                        keepPopup={this.keepAddToAlbumPopup}
                        openCreate={this.openCreateAlbumPopup}
                        userId={this.props.userId}
                        paintingId={this.state.painting.paintingId}
                        chooseAlbum={this.openConfirmPopup}/> : '';
                const createAlbum = this.state.showCreateAlbumPopup
                    ? <CreateAlbum
                        closePopup={this.closeCreateAlbumPopup}
                        keepPopup={this.keepCreateAlbumPopup}
                        title={this.state.newAlbumTitle}
                        description={this.state.newAlbumDescription}
                        error={this.state.newAlbumError}
                        handleTitle={this.handleNewAlbumTitleChange}
                        handleDesc={this.handleNewAlbumDescChange}
                        create={this.createAlbum}/> : '';
                const confirmPopup = this.state.showConfirmPopup
                    ? <ConfirmPopup
                        closePopup={this.closeConfirmPopup}
                        keepPopup={this.keepConfirmPopup}
                        question={this.state.addToAlbumQuestion}
                        confirm={this.addToAlbum}
                        decline={this.declineAddToAlbum}/> : '';

                return(
                    <div className={'art-page'}>
                        {popup}
                        {addCommentForm}
                        {addToAlbum}
                        {createAlbum}
                        {confirmPopup}
                        <div className={'art-page-left-container'}>
                            <div className={'art-page-image'}>
                                <img src={this.state.painting.imagePath} />
                                <div className={'art-page-likes-info'}>
                                    <div className={'art-page-likes-comments'}>
                                        <i className={`fa fa-heart art-page-like ${this.state.isLiked ? 'active' : ''}`} onClick={this.handleLike}/>
                                        Likes: <span className={'art-page-l'}>{this.state.likesCount}</span>
                                        | <i className={'fa fa-comment art-page-comment'} onClick={this.openCommentFrom}/>
                                        Comments: <span className={'art-page-c'}>{this.state.painting.commentsCount}</span></div>
                                    <div className={'art-page-date'}>{(new Date(this.state.painting.uploadDate)).toLocaleDateString()}</div>
                                </div>
                            </div>
                            <div className={'art-page-info'}>
                                <div className={'art-page-author-container'}>
                                    <div className={'art-page-author-title'}>
                                        <div>Title</div>
                                    </div>
                                    <div className={`art-page-author-name art-page-title`}>{this.state.painting.title}</div>
                                </div>
                                <div className={'art-page-author-container'}>
                                    <div className={'art-page-author-title'} onClick={this.handleAuthorChange}>
                                        <div>Author</div>
                                        {this.state.showAuthor ? <i className={'fa fa-chevron-up'} /> : <i className={'fa fa-chevron-down'} />}
                                    </div>
                                    <div className={`art-page-author-name ${this.state.showAuthor ? 'active' : ''}`}>{this.state.painting.painter}</div>
                                </div>
                                {materials}
                                {description}
                            </div>
                            <div className={'art-page-comments'}>
                                <div className={'art-page-author-title art-page-comments-title'}>
                                    <div>Comments</div>
                                </div>
                                <div className={'art-page-comments-container'}>
                                    {comments}
                                </div>
                            </div>
                        </div>
                        <div className={'art-page-right-container'}>
                            <div className={'art-page-auction-container'}>
                                <div className={'art-page-price'}>{this.state.painting.status ? `$${this.state.painting.price}` : 'Sold'}</div>
                                {auctionBtn}
                            </div>
                            <div className={'art-page-right-info'}>
                                <div className={'art-page-addtoalbum-btn'} onClick={this.openAddToAlbumPopup}>Add to album</div>
                                {owner}
                                <div className={'art-page-genres-container'}>
                                    {genres}
                                </div>
                            </div>
                        </div>
                    </div>
                );
            }else{
                return(<div className={'painting-not-found'}>Painting not found</div>);
            }
        }else{
            return <Loading/>;
        }
    }
}

export default PaintingPage