import React from 'react';
import '../componentsStyles/Loading.css';

export const Loading = () => {
    return (
        <div className="col-12 loading-container">
            <span className="fa fa-spinner fa-pulse fa-4x fa-fw text-warning" />
        </div>
    );
};