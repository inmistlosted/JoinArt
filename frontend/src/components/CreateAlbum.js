import React, {Component} from 'react';
import '../componentsStyles/CreateAlbum.css';

class CreateAlbum extends Component{
    constructor(props) {
        super(props);
    }

    render() {
        return(
            <div className={'create-album-container'} onClick={this.props.closePopup}>
                <div className={'create-album-inner'} onClick={this.props.keepPopup}>
                    <div className={'create-album-title'}>
                        {this.props.isEdit ? 'Edit album' : 'Create new album'}
                    </div>
                    <div className={'create-album-info'}>
                        <div className={'create-album-title-input'}>
                            <div>Title</div>
                            <input type={'text'} className={this.props.error ? 'error' : ''} placeholder={'Enter title of album'} value={this.props.title} onChange={this.props.handleTitle}/>
                            {this.props.error ? <span>Album title cannot be empty</span> : ''}
                        </div>
                        <div className={'create-album-desc'}>
                            <div>Description</div>
                            <textarea rows={7} className={'add-comment-content'} value={this.props.description} placeholder={'Enter description of album'} onChange={this.props.handleDesc}/>
                        </div>
                    </div>
                    <div className={'create-album-btn'} onClick={this.props.create}>{this.props.isEdit ? 'Edit' : 'Create album'}</div>
                </div>
            </div>
        );
    }
}

export default CreateAlbum;