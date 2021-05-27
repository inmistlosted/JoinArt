import React, {Component} from 'react';
import '../componentsStyles/AddComment.css';

class AddComment extends Component{
    constructor(props) {
        super(props);

        this.state = {
            content: ''
        };
    }

    render() {
        return(
            <div className={'add-comment-form-container'} onClick={this.props.closePopup}>
                <div className={'add-comment-form'} onClick={this.props.keepPopup}>
                    <div className={'add-comment-form-title'}>
                        Comment
                    </div>
                    <textarea rows={7} className={'add-comment-content'} value={this.props.content} placeholder={'Leave your comment'} onChange={this.props.writeComment}/>
                    <div className={'add-comment-btn'} onClick={this.props.addComment}>Add comment</div>
                </div>
            </div>
        );
    }
}

export default AddComment;