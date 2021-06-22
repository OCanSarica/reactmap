import React from 'react';
import PropTypes from 'prop-types';
import { Typography, IconButton, DialogTitle as MuiDialogTitle } from '@material-ui/core';
import { grey } from '@material-ui/core/colors';
import CloseIcon from '@material-ui/icons/Close';

export function PanelTitle(props) {

    const { children, onClose, ...other } = props;

    return (
        <MuiDialogTitle disableTypography className={style.root} {...other}>
            <Typography variant="h6">{children}</Typography>
            {onClose && (
                <IconButton aria-label="close" className={style.closeButton} onClick={onClose}>
                    <CloseIcon />
                </IconButton>
            )}
        </MuiDialogTitle>
    );
}

const style = {
    root: {
        margin: 0,
        padding: 2,
    },
    closeButton: {
        position: 'absolute',
        right: 1,
        top: 1,
        color: grey[500],
    },
}

export default PanelTitle;

PanelTitle.propTypes = {
    children: PropTypes.node,
    onClose: PropTypes.func,
};