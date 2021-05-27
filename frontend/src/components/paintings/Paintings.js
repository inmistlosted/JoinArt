import React, {Component} from 'react';
import '../../componentsStyles/Paintings.css';
import Painting from './Painting';

class Paintings extends Component{
    constructor(props) {
        super(props);

        this.state = {
            paintings: ['p1','p2','p3','p4','p5','p6','p7','p8','p9','p10','p11','p12','p13']
        };
    }

    render() {
        const {paintings} = this.state;
        const renderedPaintings = paintings.map((painting, index) => {
            return <Painting/>;
        });

        return(
            <div className={'paintings'}>
                {renderedPaintings}
            </div>
        );
    }
}

export default Paintings;