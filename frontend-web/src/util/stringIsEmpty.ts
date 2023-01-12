export default function stringIsEmpty(text: string|null|undefined): boolean {
    return text == null || text.match(/^\s*$/) !== null;
}