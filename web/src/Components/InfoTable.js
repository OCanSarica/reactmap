import React from 'react';
import PropTypes from 'prop-types';
import Icon from '@material-ui/core/Icon';
import { connect } from 'react-redux';
import { uiAction, layerAction } from '../actions'
import { Table, LoadingPanel, PoiDetail } from ".";
import { mapTool } from '../tools';

class InfoTableComponent extends React.PureComponent {

    table = {
        options: {
            search: false,
            actionsColumnIndex: 0,
            showTitle: false,
            toolbar: false,
            paging: false
        },
        style: {
            maxHeight: '500px'
        },
        columns: [
            {
                field: "key",
                hidden: true,
            },
            {
                title: "Id",
                field: "id",
            },
            {
                title: "Katman",
                field: "layer",
            },
            {
                title: "Açıklama",
                field: "label",
            },
        ],
        actions: [
            {
                icon: () => <Icon color="secondary">edit</Icon>,
                tooltip: 'Detay',
                onClick: (event, rowData) => this.showInfo(rowData)
            }]
    }

    render = () => {

        const { infoResults } = this.props;

        if (infoResults.isLoading) {

            return <LoadingPanel />;
        }
        else if (!infoResults.data) {

            return null;
        }

        return (
            <div className="info-table">
                <Table actions={this.table.actions}
                    columns={this.table.columns}
                    data={infoResults.data}
                    options={this.table.options}
                    style={this.table.style} />
            </div>
        )
    }

    componentDidMount = () => {

        this.props.getFeatureInfo(this.props.coordinates);
    }

    componentWillUnmount = () => {

        mapTool.clearMap();
    }

    showInfo = (row) => {

        if (row.layer === "Poi") {

            this.props.openRightPanel(<PoiDetail id={Number(row.key)} />, "Poi Detay");
        }
    }
}

const mapStateToProps = (state) => {

    const { infoResults } = state.layerReducer;

    return { infoResults };
}

const mapDispatchToProps = (dispatch) => {

    return {
        getFeatureInfo: (coordinates) => dispatch(layerAction.getFeatureInfo(coordinates)),
        openRightPanel: (children, title) => dispatch(uiAction.openRightPanel(children, title)),
        openBottomPanel: (children, title) => dispatch(uiAction.openBottomPanel(children, title)),
    }
}

export const InfoTable = connect(mapStateToProps, mapDispatchToProps)(InfoTableComponent);

export default InfoTable;

InfoTable.propTypes = {
    coordinates: PropTypes.array.isRequired,
};