import React from 'react';
import { PanelTitle } from "."
import { Typography, Dialog, DialogContent, DialogActions, Button } from '@material-ui/core';

export class Panel extends React.Component {

    render = () => {

        return (
            <div>
                <Dialog onClose={this.handleClose} aria-labelledby="customized-dialog-title" open={true}
                    disableBackdropClick={false}>
                    <PanelTitle id="customized-dialog-title" onClose={this.handleClose}>
                        Modal title
                </PanelTitle>
                    <DialogContent dividers>
                        <Typography gutterBottom>
                            Cras mattis consectetur purus sit amet fermentum. Cras justo odio, dapibus ac facilisis
                            in, egestas eget quam. Morbi leo risus, porta ac consectetur ac, vestibulum at eros.
                  </Typography>
                        <Typography gutterBottom>
                            Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Vivamus sagittis
                            lacus vel augue laoreet rutrum faucibus dolor auctor.
                  </Typography>
                        <Typography gutterBottom>
                            Aenean lacinia bibendum nulla sed consectetur. Praesent commodo cursus magna, vel
                            scelerisque nisl consectetur et. Donec sed odio dui. Donec ullamcorper nulla non metus
                            auctor fringilla.
                  </Typography>
                    </DialogContent>
                    <DialogActions>
                        <Button autoFocus onClick={this.handleClose} color="primary">
                            Save changes
                  </Button>
                    </DialogActions>
                </Dialog >
            </div >
        );
    }

    handleClose = () => {

        console.log("close");
    }
}

export default Panel;