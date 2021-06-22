import React from 'react';
import { BaseMap, Map, LayerControl, MainMenu, ToolMenu, BottomPanel, RightPanel } from '../components'

export class HomePage extends React.Component {

    render = () => {

        return (<div>
            <Map />
            <BaseMap />
            <LayerControl />
            <MainMenu />
            <ToolMenu />
            <BottomPanel />
            <RightPanel />
        </div>);
    }
}