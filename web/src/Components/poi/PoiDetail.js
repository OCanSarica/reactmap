import React from 'react';
import PropTypes from 'prop-types';
import { toast } from 'react-toastify';
import { TextField, Button } from '@material-ui/core';
import { connect } from 'react-redux';
import { poiAction, uiAction } from '../../actions';
import { mapTool } from '../../tools';
import { LoadingPanel } from '../';

class PoiDetailComponent extends React.PureComponent {

    state = { }

    render = () => {

        const { updatedPoi, poi } = this.props;

        if (updatedPoi.isLoading || poi.isLoading) {

            return <LoadingPanel />;
        }
        else if (updatedPoi.data) {

            mapTool.refreshWmsLayer();

            this.props.closeRightPanel();

            return null;
        }
        else if (!poi.data) {

            return null;
        }

        const { id, name } = this.state;

        return (
            <div className="updating-poi">
                <TextField value={id}
                    margin="dense"
                    label="Id"
                    disabled
                    fullWidth
                />

                <TextField value={name}
                    onKeyPress={this.handlePress}
                    onChange={this.handleChange}
                    name='name'
                    margin="dense"
                    label="İsim"
                    fullWidth
                />
                <Button style={{ marginTop: 20 }} onClick={this.update} color="primary" variant="contained">
                    Güncelle
                </Button>
            </div>
        )
    }

    componentDidMount = () => {

        this.props.get(this.props.id);
    }

    componentWillUnmount = () => {

        mapTool.clearMap();

        this.props.reset();
    }

    static getDerivedStateFromProps(props, state) {

        const poi = props.poi.data;

        if (poi && poi.id !== state.id) {

            return { id: poi.id, name: poi.name };
        }

        return null;
    }

    update = () => {

        const { name, id } = this.state;

        if (!name) {

            toast.info("İsim giriniz.");

            return;
        }

        this.props.update({ name, id });
    }

    handleChange = (e, checkbox) => {

        const { name, value, checked } = e.target;

        this.setState({ [name]: !checkbox ? value : checked });
    }

    handlePress = (e) => {

        if (e.key === 'Enter') {

            this.update();
        }
    }
}

const mapStateToProps = (state) => {

    const { updatedPoi, poi } = state.poiReducer;

    return { updatedPoi, poi };
}

const mapDispatchToProps = (dispatch) => {

    return {
        get: (id) => dispatch(poiAction.get(id)),
        update: (item) => dispatch(poiAction.update(item)),
        reset: () => dispatch(poiAction.reset()),
        closeRightPanel: () => dispatch(uiAction.closeRightPanel()),
    }
}

export const PoiDetail = connect(mapStateToProps, mapDispatchToProps)(PoiDetailComponent);

export default PoiDetail;

PoiDetail.propTypes = {
    id: PropTypes.number.isRequired,
};