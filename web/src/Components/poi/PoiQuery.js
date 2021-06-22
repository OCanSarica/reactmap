import React from 'react';
import Icon from '@material-ui/core/Icon';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { Table, PoiDetail } from "../";
import { poiService } from "../../services";
import { mapTool } from "../../tools";
import { uiAction } from "../../actions";

class PoiQueryComponent extends React.PureComponent {

    table = {
        options: {
            search: false,
            actionsColumnIndex: 0,
            pageSize: 10,
            filtering: true,
            toolbar: false,
            showTitle: false,
        },
        style: {
            maxHeight: '500px'
        },
        columns: [
            {
                title: "Id",
                field: "id",
            },
            {
                title: "İsim",
                field: "name",
            },
        ],
        actions: [
            {
                icon: () => <Icon color="secondary">edit</Icon>,
                tooltip: 'Detay',
                onClick: (event, rowData) =>
                    this.props.openRightPanel(<PoiDetail id={rowData.id} />, "Poi Detay")
            },
            {
                icon: () => <Icon color="primary">push_pin</Icon>,
                tooltip: 'Haritada Göster',
                onClick: (event, rowData) => zoom(rowData.id)
            }]
    }

    render = () => {

        return (
            <div>
                <Table columns={this.table.columns}
                    options={this.table.options}
                    actions={this.table.actions}
                    style={this.table.style}
                    data={query => new Promise((resolve, reject) => {

                        poiService.paginate({
                            filterColumns: query.filters.map(x => x.column.field),
                            filterValues: query.filters.map(x => x.value),
                            orderValues: query.orderDirection ? [query.orderDirection] : [],
                            orderColumns: query.orderBy ? [query.orderBy.field] : [],
                            limit: query.pageSize,
                            offset: (query.page) * query.pageSize
                        }).
                            then(result => {

                                resolve({
                                    data: result.data.rows,
                                    page: query.page,
                                    totalCount: result.data.count,
                                })
                            }).
                            catch(() => {

                                toast.error("Poi'ler getirilemedi.");

                                reject();
                            })
                    })
                    } />
            </div>
        )
    }
}

const zoom = (id) => {

    poiService.getGeometry(id).
        then(response => {

            if (!response.success) {

                toast.error("Geometri getirilemedi.");

                return;
            }

            mapTool.zoomToWkt(response.data, 16, "EPSG:4326");
        }).
        catch(() => {

            toast.error("Geometri getirilemedi.");
        })
}

const mapDispatchToProps = (dispatch) => {

    return {
        openRightPanel: (children, title) => dispatch(uiAction.openRightPanel(children, title))
    }
}


export const PoiQuery = connect(null, mapDispatchToProps)(PoiQueryComponent);

export default PoiQuery;
