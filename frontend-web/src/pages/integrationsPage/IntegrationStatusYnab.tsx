import React, {useEffect} from "react";
import {useQuery} from "react-query";
import {generatePath, useLocation, useNavigate} from "react-router-dom";

import Loader from "../../components/Loader";
import {
    ConnectService, DisconnectService, ReturnService, StatusService
} from "../../generated/App.Function.Integration.Ynab";
import withComponentErrorBoundary from "../../hoc/withComponentErrorBoundary";
import useQueryString from "../../hooks/queryString";
import {paths} from "../../paths";
import redirectTo from "../../util/redirect";
import stringIsEmpty from "../../util/stringIsEmpty";
import ConnectButton from "./ConnectButton";
import DisconnectButton from "./DisconnectButton";
import logo from "./integrationStatusYnab/logo.svg"
import StatusConnected from "./StatusConnected";
import StatusContainer from "./StatusContainer";
import StatusDisconnected from "./StatusDisconnected";

const getYnabReturnUrl = () => new URL(
    generatePath(paths.integrations.return.ynab),
    window.location.href
).toString();

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

const ReturnWidget = withComponentErrorBoundary(() => {
    const query = useQueryString();
    const navigate = useNavigate();
    const returnCode = query.get("code");
    const returnUrl = getYnabReturnUrl();
    
    const {isLoading, refetch} = useQuery(
        `${IntegrationStatusYnab.name}.Return`,
        () => returnCode ? ReturnService.return(returnCode, returnUrl) : null,
        {
            enabled: false,
            retry: false,
            onError: () => {
                navigate(paths.integrations.index)
            },
            onSuccess: () => {
                navigate(paths.integrations.index)
            }
        }
    );

    // Call return 
    useEffect(() => {
        if (!stringIsEmpty(returnCode)) {
            refetch().then();
        }
    }, [refetch, returnCode])

    if (isLoading) {
        return <Loader />;
    } else {
        return null;
    }
});

const StatusWidget = withComponentErrorBoundary(() => {
    const {
        data, isLoading, refetch
    } = useQuery(
        `${IntegrationStatusYnab.name}.Status`,
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

const IntegrationStatusYnab = () => {
    const location = useLocation();
    const isReturnUrl = location.pathname === paths.integrations.return.ynab;
    
    return (
        <StatusContainer icon={logo} name="ynab">
            {isReturnUrl ? <ReturnWidget/> : <StatusWidget/>}
        </StatusContainer>
    )
};

export default IntegrationStatusYnab;