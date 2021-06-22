import React from 'react';
import MaterialTable from 'material-table/'
import PropTypes from 'prop-types';
import Icon from '@material-ui/core/Icon';
import { green } from '@material-ui/core/colors';

export class Table extends React.Component {

    render = () => {

        let { title, columns, data, options, style, actions, editable, onRowClick,
            detailPanels, isLoading } = this.props;

        const defaultoptions = {
            addRowPosition: "first",
            headerstyle: {
                backgroundColor: '#f53',
            }
        }

        const tableOptions = { ...defaultoptions, ...options };

        if (!style) {

            style = {};
        }

        style.overflowY = 'auto'

        return (
            <MaterialTable title={title}
                columns={columns}
                data={data}
                options={tableOptions}
                style={style}
                actions={actions}
                localization={localization}
                editable={editable}
                onRowClick={onRowClick}
                detailPanel={detailPanels}
                isLoading={isLoading}
                icons=
                {
                    {
                        Add: () =>
                            <Icon
                                style={{ fontSize: 24, color: green[500] }}>add
                            </Icon>,
                        Edit: () =>
                            <Icon
                                color="secondary">edit
                            </Icon>,
                        Delete: () =>
                            <Icon
                                color="error">delete
                            </Icon>,
                        Check: () =>
                            <Icon
                                style={{ color: green[500] }}>check
                            </Icon>,
                        Clear: () =>
                            <Icon
                                color="error">clear
                            </Icon>,
                    }
                }
            />
        )
    }
}

const localization = {
    header: {
        actions: ''
    },
    pagination: {
        labelRowsSelect: "Kayıt",
        nextTooltip: "Sonraki Sayfa",
        previousTooltip: "Önceki Sayfa",
        firstTooltip: "İlk Sayfa",
        lastTooltip: "Son Sayfa",
    },
    body: {
        emptyDataSourceMessage: "Kayıt bulunamadı.",
        editRow: {
            deleteText: "Silmek istediğinize emin misiniz?",
            cancelTooltip: "İptal",
        },
        filterRow: {
            filterTooltip: "Filtre"
        },
        addTooltip: 'Ekle',
        editTooltip: 'Güncelle',
        deleteTooltip: 'Sil',
        saveTooltip: 'Kaydet'
    }
}

export default Table;

Table.propTypes = {
    data: PropTypes.array.isRequired,
    columns: PropTypes.array.isRequired,
    options: PropTypes.object.isRequired,
    style: PropTypes.object,
    title: PropTypes.string,
    actions: PropTypes.array,
    detailPanels: PropTypes.array,
    onRowClick: PropTypes.func,
    isLoading: PropTypes.bool
};
