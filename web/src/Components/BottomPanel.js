import React from 'react';
import { connect } from 'react-redux';
import { uiAction } from '../actions'
import { IconButton } from '@material-ui/core';
import { Close, Minimize, Maximize } from '@material-ui/icons';

class BottomPanelComponent extends React.PureComponent {

    render = () => {

        const { visible, children, title, minimize } = this.props;

        return (
            visible ?
                <div className={minimize ? "bottom-panel-minimize" : "bottom-panel"}>
                    <div className="bottom-panel-header">
                        <label className="bottom-panel-title">
                            {title}
                        </label>
                        <label className="bottom-panel-minimize-button">
                            {
                                minimize ?
                                    <IconButton onClick={this.props.show}
                                        color="primary"
                                        component="span">
                                        <Maximize />
                                    </IconButton> :
                                    <IconButton onClick={this.props.hide}
                                        color="primary"
                                        component="span">
                                        <Minimize />
                                    </IconButton>
                            }
                        </label>
                        <label className="bottom-panel-close-button">
                            <IconButton onClick={this.props.close}
                                color="primary"
                                component="span">
                                <Close />
                            </IconButton>
                        </label>
                    </div>
                    <div className="bottom-panel-body" style={minimize ? { "display": "none" } : {}}>
                        {children}
                    </div>
                </div> :
                null
        );
    }
}

const mapStateToProps = (state) => {

    const { visible, children, title, minimize } = state.uiReducer.bottomPanel;

    return { visible, children, title, minimize };
}

const mapDispatchToProps = (dispatch) => {

    return {
        close: () => dispatch(uiAction.closeBottomPanel()),
        hide: () => dispatch(uiAction.minimizeBottomPanel()),
        show: () => dispatch(uiAction.maximizeBottomPanel()),
    }
}

export const BottomPanel = connect(mapStateToProps, mapDispatchToProps)(BottomPanelComponent);

export default BottomPanel;