import {jest} from "@jest/globals";
import {act, screen, waitFor} from "@testing-library/react";
import {Simulate} from "react-dom/test-utils";

import {useYnabConnectService} from "../../data/useYnabConnectService";
import {useYnabDisconnectService} from "../../data/useYnabDisconnectService";
import {useYnabStatusService} from "../../data/useYnabStatusService";
import renderWithRouter from "../../util/testRenderWithRouter";
import IntegrationStatusYnab from "./IntegrationStatusYnab";

import click = Simulate.click;

jest.mock("../../generated/App.Function.Integration.Ynab");
jest.mock("../../data/useYnabStatusService");
jest.mock("../../data/useYnabConnectService");
jest.mock("../../data/useYnabDisconnectService");

describe(IntegrationStatusYnab.name, () => {
    const mockedUseStatusService = useYnabStatusService as jest.Mock<any>;
    const mockedUseDisconnectService = useYnabDisconnectService as jest.Mock<any>;
    const mockedUseConnectService = useYnabConnectService as jest.Mock<any>;
    
    function mockStatusDisconnected() {
        mockedUseStatusService.mockReturnValue({isLoading: false});
        mockedUseConnectService.mockReturnValue({isInitialLoading: false});
        mockedUseDisconnectService.mockReturnValue({isInitialLoading: false});
        
        mockedUseStatusService.mockReturnValue({
            isLoading: false,
            data: {data: {connected: false}}
        });
    }

    function mockStatusConnected() {
        mockedUseStatusService.mockReturnValue({isLoading: false});
        mockedUseConnectService.mockReturnValue({isInitialLoading: false});
        mockedUseDisconnectService.mockReturnValue({isInitialLoading: false});

        mockedUseStatusService.mockReturnValue({
            isLoading: false,
            data: {data: {connected: true, name: "somename"}}
        });
    }
    
    test("Render loading status", async () => {
        mockedUseStatusService.mockReturnValue({isLoading: true});
        mockedUseConnectService.mockReturnValue({isInitialLoading: false});
        mockedUseDisconnectService.mockReturnValue({isInitialLoading: false});

        renderWithRouter(<IntegrationStatusYnab />);
        
        expect(screen.getByText("label.loading")).toBeInTheDocument();
    });

    test("Render connected", async () => {
        mockStatusConnected();

        renderWithRouter(<IntegrationStatusYnab />);

        expect(screen.queryByText("label.loading")).not.toBeInTheDocument();
        expect(screen.getByText("label.connected")).toBeInTheDocument();
    });

    test("Render connected, click disconnect", async () => {
        mockStatusConnected();
        const mockedDisconnectRefetch = jest.fn();
        mockedUseDisconnectService.mockReturnValue({
            isLoading: false,
            refetch: mockedDisconnectRefetch,
        })
        
        renderWithRouter(<IntegrationStatusYnab />);

        const button = screen.getByText("button.disconnect");
        expect(button).toBeInTheDocument();
        act(() => {
            click(button);
        });
        await waitFor(() => expect(mockedDisconnectRefetch).toHaveBeenCalledTimes(1));
    });

    test("Render connected, loading disconnect button", async () => {
        mockStatusConnected();
        mockedUseDisconnectService.mockReturnValue({isInitialLoading: true})

        renderWithRouter(<IntegrationStatusYnab />);

        expect(screen.getByText("button.disconnect")).toHaveAttribute("disabled");
        expect(screen.getByText("label.connected")).toBeInTheDocument();
    });

    test("Render disconnected", async () => {
        mockStatusDisconnected();

        renderWithRouter(<IntegrationStatusYnab />);

        expect(screen.queryByText("label.loading")).not.toBeInTheDocument();
        expect(screen.getByText("label.disconnected")).toBeInTheDocument();
    });

    test("Render disconnected, click connect", async () => {
        mockStatusDisconnected();
        const mockedConnectRefetch = jest.fn();
        mockedUseConnectService.mockReturnValue({
            isLoading: false,
            refetch: mockedConnectRefetch,
        })

        renderWithRouter(<IntegrationStatusYnab />);

        const button = screen.getByText("button.connect");
        expect(button).toBeInTheDocument();
        act(() => {
            click(button);
        });

        expect(mockedConnectRefetch).toHaveBeenCalledTimes(1);
    });

    test("Render disconnected, loading connect button", async () => {
        mockStatusDisconnected();
        mockedUseConnectService.mockReturnValue({isInitialLoading: true})

        renderWithRouter(<IntegrationStatusYnab />);

        expect(screen.getByText("button.connect")).toHaveAttribute("disabled");
        expect(screen.getByText("label.disconnected")).toBeInTheDocument();
    });
});