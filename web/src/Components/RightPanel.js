import React from 'react';
import { connect } from 'react-redux';
import { IconButton } from '@material-ui/core';
import { Close } from '@material-ui/icons';
import { uiAction } from '../actions'

class RightPanelComponent extends React.PureComponent {

    state = { visible: false };

    render = () => {

        const { children, title } = this.props;

        return (
            this.state.visible ?
                <div className="right-panel">
                    <div>
                        <label className="right-panel-close-button">
                            <IconButton onClick={this.props.close}
                                color="primary"
                                component="span">
                                <Close />
                            </IconButton>
                        </label>
                        <label className="right-panel-title">
                            {title}
                        </label>
                    </div>
                    <div className="right-panel-body">
                        {children}
                    </div>
                </div> :
                null
        );
    }

    static getDerivedStateFromProps(props, state) {

        if (props.visible != state.visible) {

            return { visible: props.visible };
        }
        
        return null;
    }
}


const mapStateToProps = (state) => {

    const { visible, children, title } = state.uiReducer.rightPanel;

    return { visible, children, title };
}

const mapDispatchToProps = (dispatch) => {

    return {
        close: () => dispatch(uiAction.closeRightPanel()),
    }
}

export const RightPanel = connect(mapStateToProps, mapDispatchToProps)(RightPanelComponent);

export default RightPanel;