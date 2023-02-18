import {jest} from "@jest/globals";
import {useQuery} from "@tanstack/react-query";
import {render, screen} from "@testing-library/react";
import React from "react";
import {
    MemoryRouter, Navigate, Route, Routes
} from "react-router-dom";

import {paths} from "../../paths";
import IntegrationReturnYnab from "./IntegrationReturnYnab";

jest.mock("../../generated/App.Function.Integration.Ynab");
jest.mock("@tanstack/react-query");

const mockedUseQuery = useQuery as jest.Mock<any>;
const mockedNavigate = jest.fn(Navigate);

describe(IntegrationReturnYnab.name, () => {
    const renderWithReturnCode = (returnCode: string) => render(
        <MemoryRouter initialEntries={[`${paths.integrations.return.ynab}?code=${returnCode}`]}>
            <Routes>
                <Route path={paths.integrations.return.ynab} element={<IntegrationReturnYnab />} />
                <Route path={paths.integrations.index} element="IntegrationsIndex" />
                <Route path={paths.errorNotFound} element="NotFound" />
            </Routes>
        </MemoryRouter>
    );
    
    test("Render loading", async () => {
        mockedUseQuery.mockReturnValue({isLoading: true});

        renderWithReturnCode("123");
        
        expect(mockedNavigate).toBeCalledTimes(0);
        expect(screen.queryByText("IntegrationsIndex")).not.toBeInTheDocument();
    });

    test("Navigate after loading", async () => {
        mockedUseQuery.mockReturnValue({isLoading: false});

        renderWithReturnCode("123");

        expect(mockedNavigate).toBeCalledTimes(0);
        expect(screen.getByText("IntegrationsIndex")).toBeInTheDocument();
    });

    test("Error on missing return code", async () => {
        mockedUseQuery.mockReturnValue({isLoading: true});

        renderWithReturnCode("");
        
        expect(mockedNavigate).toBeCalledTimes(0);
        expect(screen.getByText("NotFound")).toBeInTheDocument();
    });
});
