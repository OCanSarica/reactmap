import React from 'react';
import { connect } from 'react-redux';
import { Menu, Dropdown } from 'semantic-ui-react'
import { uiAction } from '../actions'
import { mapTool } from '../tools';
import { PoiQuery } from './';

class MainMenuComponent extends React.Component {

    render = () => {

        const { user } = this.props.user;

        return (
            <div className="main-menu">
                <Menu>
                    <Menu.Item>
                        <img className="menu-logo" src='/logo.png' alt="menu" />
                    </Menu.Item>
                    <Dropdown item text='Poi'>
                        <Dropdown.Menu>
                            <Dropdown.Item onClick={this.addPoiOnClick}>Poi Ekle</Dropdown.Item>
                            <Dropdown.Item onClick={this.queryPoiOnClick}>Poi Sorgula</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                    <Dropdown item text='Hesap'>
                        <Dropdown.Menu>
                            <Dropdown.Header>{user.name + " " + user.surname}</Dropdown.Header>
                            <Dropdown.Item>Yönetici Paneli</Dropdown.Item>
                            <Dropdown.Item>Şifre Değiştir</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Menu>
            </div>)
    }

    addPoiOnClick = () => mapTool.enableInteraction("poi_interaction");

    queryPoiOnClick = () => this.props.openBottomPanel(<PoiQuery />, "Poi Sorgula")
}

const mapStateToProps = (state) => {

    const { user } = state.authenticationReducer;

    return { user };
}

const mapDispatchToProps = (dispatch) => {

    return {
        openRightPanel: (children) => dispatch(uiAction.openRightPanel(children)),
        openBottomPanel: (children, title) => dispatch(uiAction.openBottomPanel(children, title)),
    }
}

export const MainMenu = connect(mapStateToProps, mapDispatchToProps)(MainMenuComponent);

export default MainMenu;