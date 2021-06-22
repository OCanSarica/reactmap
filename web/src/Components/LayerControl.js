import React from 'react';
import Tree from 'rc-tree';
import { connect } from 'react-redux';
import { Tabs, Tab, AppBar, Slider, Grid, IconButton } from '@material-ui/core';
import { Close, Layers } from '@material-ui/icons';
import "rc-tree/assets/index.css"
import { mapTool } from '../tools';
import { layerService } from '../services';
import { TabPanel } from '.';
import { layerAction } from '../actions'

class LayerControlComponent extends React.PureComponent {

    state = {
        defaultCheckedLayers: [],
        treeData: null,
        selectedTab: 0,
        visible: false
    };

    activeLayers = [];

    selectedLayer = null;

    render = () => {

        const { treeData, defaultCheckedLayers, selectedTab,
            title, opacity, visibilityRange, legendSource, visible } = this.state;

        if (!treeData) {

            return null;
        }

        this.initializeWmsLayer();

        return (
            visible ?
                <div>
                    <label className="layer-control-close-button">
                        <IconButton onClick={() => this.setState({ visible: false })}
                            color="primary"
                            aria-label="upload picture"
                            component="span">
                            <Close />
                        </IconButton>
                    </label>
                    <div className="layer-control">
                        <div className="layer-tree">
                            <Tree checkable
                                draggable
                                defaultExpandAll
                                onDrop={this.layerOnDrop}
                                onSelect={this.layerOnSelect}
                                onCheck={this.layerOnCheck}
                                defaultCheckedKeys={defaultCheckedLayers.map(x => x.key)}
                                treeData={treeData}
                            />
                        </div>
                        {title &&
                            <div className="layer-detail" style={{ flexGrow: 1 }}>
                                <AppBar position="static">
                                    <Tabs value={selectedTab}
                                        onChange={this.tabHandleChange}
                                        aria-label="simple tabs example">
                                        <Tab label="Görünüm" {...applyTabProps(0)} />
                                        <Tab label="Lejant" {...applyTabProps(1)} />
                                    </Tabs>
                                </AppBar>
                                <TabPanel value={selectedTab} index={0}>
                                    <Grid container
                                        spacing={2}
                                        direction="column" >
                                        <Grid item>
                                            {title}
                                        </Grid >
                                        <Grid item>
                                            <Grid container spacing={5} direction="column">
                                                <Grid item>
                                                    Saydamlık
                                    </Grid>
                                                <Grid item>
                                                    <Slider value={opacity}
                                                        onChange={this.opacityHandleChange}
                                                        valueLabelDisplay="on" />
                                                </Grid>
                                            </Grid>
                                        </Grid >
                                        <Grid item>
                                            <Grid container spacing={5} direction="column">
                                                <Grid item>
                                                    Görünürlük Aralığı
                                    </Grid>
                                                <Grid item>
                                                    <Slider value={visibilityRange}
                                                        max={21} min={1}
                                                        onChange={this.visibilityHandleChange}
                                                        valueLabelDisplay="on" />
                                                </Grid>
                                            </Grid>
                                        </Grid >
                                    </Grid>
                                </TabPanel>
                                <TabPanel value={selectedTab} index={1}>
                                    <div className="legend-img-div">
                                        <img src={legendSource} id="legend-img" alt="legend" />

                                    </div>
                                </TabPanel>
                            </div>}
                    </div>
                </div> :
                <label className="layer-control-open-button">
                    <IconButton onClick={() => this.setState({ visible: true })}
                        color="primary"
                        component="span">
                        <Layers />
                    </IconButton>
                </label>

        );
    }

    componentDidMount = () => {

        this.props.getAll();
    }

    static getDerivedStateFromProps(props, state) {

        if (!state.treeData && props.layers.data) {

            const activeLayers = [];

            const layers = [...props.layers.data];

            while (layers.length > 0) {

                const current = layers.pop();

                if (current.children.length === 0) {

                    if (current.checked) {

                        activeLayers.push(current);

                        continue;
                    }
                }

                for (const child of current.children) {

                    layers.push(child);
                }
            }

            const state = {
                treeData: props.layers.data,
                defaultCheckedLayers: activeLayers,
            }

            return state;
        }

        return null;
    }

    initializeWmsLayer = () => {

        if (!mapTool.findLayer("wms_layer")) {

            this.activeLayers = this.state.defaultCheckedLayers;

            const wmsLayer = mapTool.createTileLayer({
                name: "wms_layer",
                source: mapTool.createTileLayerSource({
                    urls: proxyUrls,
                    params: this.getCurrentLayerParams()
                })
            });

            mapTool.addLayer(wmsLayer);

            mapTool.getMap().on("moveend", () => this.refreshWmsLayer());

            document.addEventListener('keydown', mapTool.shortCut, false);
        }
    }

    refreshWmsLayer = () =>
        mapTool.updateLayerSource("wms_layer", this.getCurrentLayerParams());

    getCurrentLayerParams = () => {

        const zoomLevel = mapTool.getZoom();

        const currentLayers = this.activeLayers.filter(x =>
            x.maxVisibleLevel >= zoomLevel &&
            x.minVisibleLevel <= zoomLevel);

        return {
            layers: currentLayers.map(x => x.key),
            opacities: currentLayers.map(x => x.opacity),
            cqlFilters: currentLayers.map(x => x.cqlFilter)
        }
    }

    layerOnDrop = (info) => {

        if (info.node.parentId !== info.dragNode.parentId) {

            return;
        }

        const dropKey = info.node.key;

        const dragKey = info.dragNode.key;

        const dropPos = info.node.pos.split('-');

        const dragPos = info.dragNode.pos.split('-');

        let up = false;

        if (dropPos[dropPos.length - 1] < dragPos[dragPos.length - 1]) {

            up = true;
        }

        const _loop = (data, key, callback) => {

            data.forEach((item, index, arr) => {

                if (item.key === key) {

                    callback(item, index, arr);

                    return;
                }

                if (item.children) {

                    _loop(item.children, key, callback);
                }
            });
        };

        const data = [...this.state.treeData];

        let dragObj;

        _loop(data, dragKey, (item, index, arr) => {

            arr.splice(index, 1);

            dragObj = item;
        });

        let ar, i;

        _loop(data, dropKey, (item, index, arr) => {

            ar = arr;

            i = index;
        });

        if (up) {

            ar.splice(i, 0, dragObj);

        } else {

            ar.splice(i + 1, 0, dragObj);
        }

        const activeLayers = [];

        const layers = [...data];

        while (layers.length > 0) {

            const current = layers.pop();

            if (current.children.length === 0) {

                if (current.checked) {

                    const layer = this.activeLayers.find(x => x.key === current.key);

                    activeLayers.push(layer ?? current);

                    continue;
                }
            }

            for (const child of current.children) {

                layers.push(child);
            }
        }

        this.activeLayers = activeLayers;

        this.setState({
            treeData: data
        });

        this.refreshWmsLayer();
    };

    layerOnSelect = (selectedKeys, info) => {

        if (info.node.children.length > 0) {

            this.selectedLayer = null;

            this.setState({ title: null });

            return;
        }

        this.selectedLayer = info.node;

        let layer = this.activeLayers.find(x => x.key === info.node.key);

        if (!layer) {

            layer = info.node;
        }

        this.setState({
            title: layer.title,
            opacity: layer.opacity,
            visibilityRange: [layer.minVisibleLevel, layer.maxVisibleLevel],
            legendSource: layerService.getLegendSource(layer.key)
        });
    };

    layerOnCheck = (checkedKeys, info) => {

        if (info.node.key === this.selectedLayer?.key) {

            this.selectedLayer.checked = checkedKeys.includes(info.node.key);
        }

        const activeLayers = [];

        const layers = [...this.state.treeData];

        while (layers.length > 0) {

            const current = layers.pop();

            if (current.children.length === 0) {

                if (checkedKeys.includes(current.key)) {

                    const layer = this.activeLayers.find(x => x.key === current.key);

                    activeLayers.push(layer ?? current);

                    continue;
                }
            }

            for (const child of current.children) {

                layers.push(child);
            }
        }

        this.activeLayers = activeLayers;

        this.refreshWmsLayer();
    };

    tabHandleChange = (event, newValue) => {

        this.setState({ selectedTab: newValue });
    }

    opacityHandleChange = (event, newValue) => {

        if (!this.selectedLayer.checked) {

            return;
        }

        this.activeLayers = this.activeLayers.
            map(x => x.key !== this.selectedLayer.key ? x : { ...x, ...{ opacity: newValue } })

        this.setState({ opacity: newValue });

        this.refreshWmsLayer();
    }

    visibilityHandleChange = (event, newValue) => {

        if (!this.selectedLayer.checked) {

            return;
        }

        this.activeLayers = this.activeLayers.
            map(x => x.key !== this.selectedLayer.key ? x :
                {
                    ...x,
                    ...{
                        minVisibleLevel: newValue[0],
                        maxVisibleLevel: newValue[1]
                    }
                })

        this.setState({ visibilityRange: newValue });

        this.refreshWmsLayer();
    }
}

const proxyUrls = [window.MAP_API_URL + "/api/proxy/getmap?"];

const applyTabProps = (index) => {

    return {
        id: `simple-tab-${index}`,
        'aria-controls': `simple-tabpanel-${index}`,
    };
}

const mapStateToProps = state => {

    const { layers } = state.layerReducer;

    return { layers };
}

const mapDispatchToProps = (dispatch) => {

    return {
        getAll: () => dispatch(layerAction.getAll()),
    }
}

export const LayerControl = connect(mapStateToProps, mapDispatchToProps)(LayerControlComponent);

export default LayerControl;

