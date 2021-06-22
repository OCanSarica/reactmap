import React from 'react';
import { Menu, Icon, Popup } from 'semantic-ui-react'
import { mapTool } from '../tools';

export class ToolMenu extends React.Component {

    render = () => {

        return (
            <div className="tool-menu">
                <Menu icon>
                    <Popup content="Haritayı Temizle" trigger={
                        <Menu.Item onClick={() => mapTool.clearMap()}>
                            <Icon name='eraser' />
                        </Menu.Item>
                    } />
                    <Popup content="Geri" trigger={
                        <Menu.Item onClick={() => mapTool.mapBack()}>
                            <Icon name='backward' />
                        </Menu.Item>
                    } />
                     <Popup content="İleri" trigger={
                        <Menu.Item onClick={() => mapTool.mapForward()}>
                            <Icon name='forward' />
                        </Menu.Item>
                    } />
                    <Popup content="Bilgi Al" trigger={
                        <Menu.Item onClick={() => mapTool.enableInteraction("info_interaction")}>
                            <Icon name='info' />
                        </Menu.Item>
                    } />
                    <Popup content="İlk Görünüm" trigger={
                        <Menu.Item onClick={() => mapTool.resetMap()}>
                            <Icon name='eye' />
                        </Menu.Item>
                    } />
                    <Popup content="Pan" trigger={
                        <Menu.Item onClick={() => mapTool.pan()}>
                            <Icon name='hand paper' />
                        </Menu.Item>
                    } />
                    <Popup content="Ekran Görüntüsü" trigger={
                        <Menu.Item onClick={downloadImage}>
                            <Icon name='image' />
                        </Menu.Item>
                    } />
                    <Popup content="Haritayı Yakınlaştır" trigger={
                        <Menu.Item onClick={() =>
                            mapTool.setZoomLevel({ zoomLevel: mapTool.getZoom() + 1 })}>
                            <Icon name='zoom-in' />
                        </Menu.Item>
                    } />
                    <Popup content="Haritayı Uzaklaştır" trigger={
                        <Menu.Item onClick={() =>
                            mapTool.setZoomLevel({ zoomLevel: mapTool.getZoom() - 1 })}>
                            <Icon name='zoom-out' />
                        </Menu.Item>
                    } />
                </Menu>
            </div >)
    }
}

const downloadImage = () => {

    let canvas = document.getElementsByClassName('ol-unselectable')[0].
        firstElementChild.firstElementChild;

    let link = document.createElement('a'), e;

    link.download = "img";

    link.href = canvas.toDataURL('image/png;base64');

    if (document.createEvent) {

        e = document.createEvent('MouseEvents');

        e.initMouseEvent('click', true, true, window,
            0, 0, 0, 0, 0, false, false, false,
            false, 0, null);

        link.dispatchEvent(e);
    }
}

export default ToolMenu;