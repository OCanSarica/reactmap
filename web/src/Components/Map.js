import React from 'react';
import { connect } from 'react-redux';
import { InfoTable, PoiAdd } from ".";
import { mapTool, baseMapTool } from "../tools";
import { uiAction } from '../actions'

class MapComponent extends React.Component {

    render = () => {

        return <div id='map' />;
    }

    componentDidMount = () => {

        mapTool.createMap();

        baseMapTool.addBaseMap();

        addDefaultLayers();

        this.addDefaultInteractions();

        mapTool.getMap().on('moveend', () => mapTool.updatePermalink());
    }

    openInfoWindow = (event) => {

        event.target.setActive(false);

        const coordinates = event.feature.getGeometry().getCoordinates();

        this.props.openBottomPanel(<InfoTable coordinates={coordinates} />, "Bilgi Al")
    }

    openAddingPoiWindow = (event) => {

        event.target.setActive(false);

        this.props.openRightPanel(<PoiAdd geometry={event.feature} />, "Poi Ekle");
    }

    addDefaultInteractions = () => {

        const infoInteraction = mapTool.createDrawInteraction(
            "info_interaction",
            "info_layer",
            "Point");

        infoInteraction.on("drawend", this.openInfoWindow);

        mapTool.addInteraction(infoInteraction);

        const poiInteraction = mapTool.createDrawInteraction(
            "poi_interaction",
            "draw_layer",
            "Point");

        poiInteraction.on("drawend", this.openAddingPoiWindow);

        mapTool.addInteraction(poiInteraction);
    }
}

const addDefaultLayers = () => {

    const infoLayer = mapTool.createVectorLayer({
        name: "info_layer",
        source: mapTool.createVectorLayerSource({})
    });

    mapTool.addLayer(infoLayer);

    const drawLayer = mapTool.createVectorLayer({
        name: "draw_layer",
        source: mapTool.createVectorLayerSource({})
    });

    mapTool.addLayer(drawLayer);
}

const mapDispatchToProps = (dispatch, ownProps) => {

    return {
        openBottomPanel: (children, title) => dispatch(uiAction.openBottomPanel(children, title)),
        openRightPanel: (children, title) => dispatch(uiAction.openRightPanel(children, title)),
    }
}

export const Map = connect(null, mapDispatchToProps)(MapComponent);

export default Map;