import React from "react";
import {useQuery} from "react-query";

import Loader from "../../components/Loader";
import {ConnectService, DisconnectService, StatusService} from "../../generated/App.Function.Integration.Ynab";
import withComponentErrorBoundary from "../../hoc/withComponentErrorBoundary";
import redirectTo from "../../util/redirect";
import ConnectButton from "./ConnectButton";
import DisconnectButton from "./DisconnectButton";
import {getYnabReturnUrl} from "./IntegrationReturnYnab";
import logo from "./integrationStatusYnab/logo.svg"
import StatusConnected from "./StatusConnected";
import StatusContainer from "./StatusContainer";
import StatusDisconnected from "./StatusDisconnected";

const YnabConnectButton = withComponentErrorBoundary(() => {
    const returnUri = getYnabReturnUrl();
    
    function onConnect() {
        ConnectService
            .connect(returnUri)
            .then(response => {
                if (response.data) {
                    redirectTo(response.data);
                }
            });
    }

    return <ConnectButton onClick={() => onConnect()} />;
});

function YnabDisconnectButton(props: {onDisconnect: () => void}) {
    function onDisconnect() {
        DisconnectService
            .disconnect()
            .then(props.onDisconnect);
    }

    return <DisconnectButton onClick={() => onDisconnect()} />;
}

const StatusWidget = withComponentErrorBoundary(() => {
    const {
        data, isLoading, refetch
    } = useQuery(
        IntegrationStatusYnab.name,
        () => StatusService.status()
    );
    
    if (isLoading) {
        return <Loader />;
    }

    if (data?.data?.connected) {
        return <StatusConnected><YnabDisconnectButton onDisconnect={refetch} /></StatusConnected>;
    } else {
        return <StatusDisconnected><YnabConnectButton /></StatusDisconnected>;
    }
});

const IntegrationStatusYnab = () => (
    <StatusContainer icon={logo} name="ynab">
        <StatusWidget/>
    </StatusContainer>
);

export default IntegrationStatusYnab;