import {act, render, screen} from "@testing-library/react";
import {Simulate} from "react-dom/test-utils";

import {MenuItem} from "../menu";
import AppMenuItem from "./MenuItem";
import click = Simulate.click;
import {useNavigate} from "react-router-dom";

jest.mock("react-router-dom");
const mockedNavigate = jest.fn();

const item: MenuItem = {
    icon: <i>icon</i>,
    label: "title",
    path: "/somepath"
}

test("Render without error", () => {
    render(<AppMenuItem item={item} />)
});

test("Press item and navigate to link", () => {
    jest.mocked(useNavigate).mockImplementation(() => mockedNavigate);
    
    render(<AppMenuItem item={item} />)

    const button = screen.getByText(item.label);
    expect(button).toBeInTheDocument();
    
    act(() => click(button));
    
    expect(mockedNavigate).toHaveBeenCalledWith(item.path);
});
