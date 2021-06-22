import React from 'react';
import PropTypes from 'prop-types';
import { toast } from 'react-toastify';
import { TextField, Button } from '@material-ui/core';
import { connect } from 'react-redux';
import { poiAction, uiAction } from '../../actions';
import { mapTool } from '../../tools';
import { LoadingPanel } from '..';

class PoiAddComponent extends React.PureComponent {

    state = { name: "" };

    render = () => {

        const { addedPoi } = this.props;

        if (addedPoi.isLoading) {

            return <LoadingPanel />;
        }
        else if (addedPoi.data) {

            this.props.closeRightPanel();

            mapTool.clearMap();
            
            mapTool.refreshWmsLayer();

            return "";
        }

        return (
            <div className="add-poi">
                <TextField value={this.state.name}
                    onKeyPress={this.handlePress}
                    onChange={this.handleChange}
                    name='name'
                    margin="dense"
                    label="İsim"
                    fullWidth
                />
                <Button style={{ marginTop: 20 }} onClick={this.add} color="primary" variant="contained">
                    Ekle
                </Button>
            </div>
        )
    }

    componentWillUnmount = () => {

        mapTool.clearMap();

        this.props.reset();
    }

    add = () => {

        const { name } = this.state;

        if (!name) {

            toast.info("İsim giriniz.");

            return;
        }

        this.props.add({ name, wkt: mapTool.convertToWkt(this.props.geometry, "EPSG:4326") });
    }

    handleChange = (e, checkbox) => {

        const { name, value, checked } = e.target;

        this.setState({ [name]: !checkbox ? value : checked });
    }

    handlePress = (e) => {

        if (e.key === 'Enter') {

            this.add();
        }
    }
}

const mapStateToProps = (state) => {

    const { addedPoi } = state.poiReducer;

    return { addedPoi };
}

const mapDispatchToProps = (dispatch) => {

    return {
        add: (item) => dispatch(poiAction.add(item)),
        reset: () => dispatch(poiAction.reset()),
        closeRightPanel: () => dispatch(uiAction.closeRightPanel()),
    }
}

export const PoiAdd = connect(mapStateToProps, mapDispatchToProps)(PoiAddComponent);

export default PoiAdd;

PoiAdd.propTypes = {
    geometry: PropTypes.object.isRequired,
};