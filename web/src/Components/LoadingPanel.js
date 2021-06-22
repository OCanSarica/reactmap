import React from 'react';
import ReactLoading from "react-loading";

export class LoadingPanel extends React.PureComponent {

    render = () => {

        return (
            <div className="loading">
                <ReactLoading className="react-loading" type="spin" color="#3f51b5" />
            </div>
        );
    }
}

export default LoadingPanel;
