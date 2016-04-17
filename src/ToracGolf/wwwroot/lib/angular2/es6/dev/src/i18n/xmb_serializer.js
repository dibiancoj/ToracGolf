import { isPresent, isBlank, RegExpWrapper } from 'angular2/src/facade/lang';
import { HtmlElementAst } from 'angular2/src/compiler/html_ast';
import { id } from './message';
import { HtmlParser } from 'angular2/src/compiler/html_parser';
import { ParseError } from 'angular2/src/compiler/parse_util';
let _PLACEHOLDER_REGEXP = RegExpWrapper.create(`\\<ph(\\s)+name=("(\\w)+")\\/\\>`);
const _ID_ATTR = "id";
const _MSG_ELEMENT = "msg";
const _BUNDLE_ELEMENT = "message-bundle";
export function serializeXmb(messages) {
    let ms = messages.map((m) => _serializeMessage(m)).join("");
    return `<message-bundle>${ms}</message-bundle>`;
}
export class XmbDeserializationResult {
    constructor(content, messages, errors) {
        this.content = content;
        this.messages = messages;
        this.errors = errors;
    }
}
export class XmbDeserializationError extends ParseError {
    constructor(span, msg) {
        super(span, msg);
    }
}
export function deserializeXmb(content, url) {
    let parser = new HtmlParser();
    let normalizedContent = _expandPlaceholder(content.trim());
    let parsed = parser.parse(normalizedContent, url);
    if (parsed.errors.length > 0) {
        return new XmbDeserializationResult(null, {}, parsed.errors);
    }
    if (_checkRootElement(parsed.rootNodes)) {
        return new XmbDeserializationResult(null, {}, [new XmbDeserializationError(null, `Missing element "${_BUNDLE_ELEMENT}"`)]);
    }
    let bundleEl = parsed.rootNodes[0]; // test this
    let errors = [];
    let messages = {};
    _createMessages(bundleEl.children, messages, errors);
    return (errors.length == 0) ?
        new XmbDeserializationResult(normalizedContent, messages, []) :
        new XmbDeserializationResult(null, {}, errors);
}
function _checkRootElement(nodes) {
    return nodes.length < 1 || !(nodes[0] instanceof HtmlElementAst) ||
        nodes[0].name != _BUNDLE_ELEMENT;
}
function _createMessages(nodes, messages, errors) {
    nodes.forEach((item) => {
        if (item instanceof HtmlElementAst) {
            let msg = item;
            if (msg.name != _MSG_ELEMENT) {
                errors.push(new XmbDeserializationError(item.sourceSpan, `Unexpected element "${msg.name}"`));
                return;
            }
            let id = _id(msg);
            if (isBlank(id)) {
                errors.push(new XmbDeserializationError(item.sourceSpan, `"${_ID_ATTR}" attribute is missing`));
                return;
            }
            messages[id] = msg.children;
        }
    });
}
function _id(el) {
    let ids = el.attrs.filter(a => a.name == _ID_ATTR);
    return ids.length > 0 ? ids[0].value : null;
}
function _serializeMessage(m) {
    let desc = isPresent(m.description) ? ` desc='${m.description}'` : "";
    return `<msg id='${id(m)}'${desc}>${m.content}</msg>`;
}
function _expandPlaceholder(input) {
    return RegExpWrapper.replaceAll(_PLACEHOLDER_REGEXP, input, (match) => {
        let nameWithQuotes = match[2];
        return `<ph name=${nameWithQuotes}></ph>`;
    });
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoieG1iX3NlcmlhbGl6ZXIuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJkaWZmaW5nX3BsdWdpbl93cmFwcGVyLW91dHB1dF9wYXRoLW9YRE80cDJ2LnRtcC9hbmd1bGFyMi9zcmMvaTE4bi94bWJfc2VyaWFsaXplci50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiT0FBTyxFQUFDLFNBQVMsRUFBRSxPQUFPLEVBQUUsYUFBYSxFQUFDLE1BQU0sMEJBQTBCO09BQ25FLEVBQVUsY0FBYyxFQUFDLE1BQU0sZ0NBQWdDO09BQy9ELEVBQVUsRUFBRSxFQUFDLE1BQU0sV0FBVztPQUM5QixFQUFDLFVBQVUsRUFBQyxNQUFNLG1DQUFtQztPQUNyRCxFQUFrQixVQUFVLEVBQUMsTUFBTSxrQ0FBa0M7QUFFNUUsSUFBSSxtQkFBbUIsR0FBRyxhQUFhLENBQUMsTUFBTSxDQUFDLGtDQUFrQyxDQUFDLENBQUM7QUFDbkYsTUFBTSxRQUFRLEdBQUcsSUFBSSxDQUFDO0FBQ3RCLE1BQU0sWUFBWSxHQUFHLEtBQUssQ0FBQztBQUMzQixNQUFNLGVBQWUsR0FBRyxnQkFBZ0IsQ0FBQztBQUV6Qyw2QkFBNkIsUUFBbUI7SUFDOUMsSUFBSSxFQUFFLEdBQUcsUUFBUSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsS0FBSyxpQkFBaUIsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQztJQUM1RCxNQUFNLENBQUMsbUJBQW1CLEVBQUUsbUJBQW1CLENBQUM7QUFDbEQsQ0FBQztBQUVEO0lBQ0UsWUFBbUIsT0FBZSxFQUFTLFFBQW9DLEVBQzVELE1BQW9CO1FBRHBCLFlBQU8sR0FBUCxPQUFPLENBQVE7UUFBUyxhQUFRLEdBQVIsUUFBUSxDQUE0QjtRQUM1RCxXQUFNLEdBQU4sTUFBTSxDQUFjO0lBQUcsQ0FBQztBQUM3QyxDQUFDO0FBRUQsNkNBQTZDLFVBQVU7SUFDckQsWUFBWSxJQUFxQixFQUFFLEdBQVc7UUFBSSxNQUFNLElBQUks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QUNMLENBQUM7QUFFRCxhQUFhLEVBQWtCO0lBQzdCLElBQUksR0FBRyxHQUFHLEVBQUUsQ0FBQyxLQUFLLENBQUMsTUFBTSxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsSUFBSSxJQUFJLFFBQVEsQ0FBQyxDQUFDO0lBQ25ELE1BQU0sQ0FBQyxHQUFHLENBQUMsTUFBTSxHQUFHLENBQUMsR0FBRyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxHQUFHLElBQUksQ0FBQztBQUM5QyxDQUFDO0FBRUQsMkJBQTJCLENBQVU7SUFDbkMsSUFBSSxJQUFJLEdBQUcsU0FBUyxDQUFDLENBQUMsQ0FBQyxXQUFXLENBQUMsR0FBRyxVQUFVLENBQUMsQ0FBQyxXQUFXLEdBQUcsR0FBRyxFQUFFLENBQUM7SUFDdEUsTUFBTSxDQUFDLFlBQVksRUFBRSxDQUFDLENBQUMsQ0FBQyxJQUFJLElBQUksSUFBSSxDQUFDLENBQUMsT0FBTyxRQUFRLENBQUM7QUFDeEQsQ0FBQztBQUVELDRCQUE0QixLQUFhO0lBQ3ZDLE1BQU0sQ0FBQyxhQUFhLENBQUMsVUFBVSxDQUFDLG1CQUFtQixFQUFFLEtBQUssRUFBRSxDQUFDLEtBQUs7UUFDaEUsSUFBSSxjQUFjLEdBQUcsS0FBSyxDQUFDLENBQUMsQ0FBQyxDQUFDO1FBQzlCLE1BQU0sQ0FBQyxZQUFZLGNBQWMsUUFBUSxDQUFDO0lBQzVDLENBQUMsQ0FBQyxDQUFDO0FBQ0wsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7aXNQcmVzZW50LCBpc0JsYW5rLCBSZWdFeHBXcmFwcGVyfSBmcm9tICdhbmd1bGFyMi9zcmMvZmFjYWRlL2xhbmcnO1xuaW1wb3J0IHtIdG1sQXN0LCBIdG1sRWxlbWVudEFzdH0gZnJvbSAnYW5ndWxhcjIvc3JjL2NvbXBpbGVyL2h0bWxfYXN0JztcbmltcG9ydCB7TWVzc2FnZSwgaWR9IGZyb20gJy4vbWVzc2FnZSc7XG5pbXBvcnQge0h0bWxQYXJzZXJ9IGZyb20gJ2FuZ3VsYXIyL3NyYy9jb21waWxlci9odG1sX3BhcnNlcic7XG5pbXBvcnQge1BhcnNlU291cmNlU3BhbiwgUGFyc2VFcnJvcn0gZnJvbSAnYW5ndWxhcjIvc3JjL2NvbXBpbGVyL3BhcnNlX3V0aWwnO1xuXG5sZXQgX1BMQUNFSE9MREVSX1JFR0VYUCA9IFJlZ0V4cFdyYXBwZXIuY3JlYXRlKGBcXFxcPHBoKFxcXFxzKStuYW1lPShcIihcXFxcdykrXCIpXFxcXC9cXFxcPmApO1xuY29uc3QgX0lEX0FUVFIgPSBcImlkXCI7XG5jb25zdCBfTVNHX0VMRU1FTlQgPSBcIm1zZ1wiO1xuY29uc3QgX0JVTkRMRV9FTEVNRU5UID0gXCJtZXNzYWdlLWJ1bmRsZVwiO1xuXG5leHBvcnQgZnVuY3Rpb24gc2VyaWFsaXplWG1iKG1lc3NhZ2VzOiBNZXNzYWdlW10pOiBzdHJpbmcge1xuICBsZXQgbXMgPSBtZXNzYWdlcy5tYXAoKG0pID0+IF9zZXJpYWxpemVNZXNzYWdlKG0pKS5qb2luKFwiXCIpO1xuICByZXR1cm4gYDxtZXNzYWdlLWJ1bmRsZT4ke21zfTwvbWVzc2FnZS1idW5kbGU+YDtcbn1cblxuZXhwb3J0IGNsYXNzIFhtYkRlc2VyaWFsaXphdGlvblJlc3VsdCB7XG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBjb250ZW50OiBzdHJpbmcsIHB1YmxpYyBtZXNzYWdlczoge1trZXk6IHN0cmluZ106IEh0bWxBc3RbXX0sXG4gICAgICAgICAgICAgIHB1YmxpYyBlcnJvcnM6IFBhcnNlRXJyb3JbXSkge31cbn1cblxuZXhwb3J0IGNsYXNzIFhtYkRlc2VyaWFsaXphdGlvbkVycm9yIGV4dGVuZHMgUGFyc2VFcnJvciB7XG4gIGNvbnN0cnVjdG9yKHNwYW46IFBhcnNlU291cmNlU3BhbiwgbXNnOiBzdHJpbmcpIHsgc3VwZXIoc3BhbiwgbXNnKTsgfVxufVxuXG5leHBvcnQgZnVuY3Rpb24gZGVzZXJpYWxpemVYbWIoY29udGVudDogc3RyaW5nLCB1cmw6IHN0cmluZyk6IFhtYkRlc2VyaWFsaXphdGlvblJlc3VsdCB7XG4gIGxldCBwYXJzZXIgPSBuZXcgSHRtbFBhcnNlcigpO1xuICBsZXQgbm9ybWFsaXplZENvbnRlbnQgPSBfZXhwYW5kUGxhY2Vob2xkZXIoY29udGVudC50cmltKCkpO1xuICBsZXQgcGFyc2VkID0gcGFyc2VyLnBhcnNlKG5vcm1hbGl6ZWRDb250ZW50LCB1cmwpO1xuXG4gIGlmIChwYXJzZWQuZXJyb3JzLmxlbmd0aCA+IDApIHtcbiAgICByZXR1cm4gbmV3IFhtYkRlc2VyaWFsaXphdGlvblJlc3VsdChudWxsLCB7fSwgcGFyc2VkLmVycm9ycyk7XG4gIH1cblxuICBpZiAoX2NoZWNrUm9vdEVsZW1lbnQocGFyc2VkLnJvb3ROb2RlcykpIHtcbiAgICByZXR1cm4gbmV3IFhtYkRlc2VyaWFsaXphdGlvblJlc3VsdChcbiAgICAgICAgbnVsbCwge30sIFtuZXcgWG1iRGVzZXJpYWxpemF0aW9uRXJyb3IobnVsbCwgYE1pc3NpbmcgZWxlbWVudCBcIiR7X0JVTkRMRV9FTEVNRU5UfVwiYCldKTtcbiAgfVxuXG4gIGxldCBidW5kbGVFbCA9IDxIdG1sRWxlbWVudEFzdD5wYXJzZWQucm9vdE5vZGVzWzBdOyAgLy8gdGVzdCB0aGlzXG4gIGxldCBlcnJvcnMgPSBbXTtcbiAgbGV0IG1lc3NhZ2VzOiB7W2tleTogc3RyaW5nXTogSHRtbEFzdFtdfSA9IHt9O1xuXG4gIF9jcmVhdGVNZXNzYWdlcyhidW5kbGVFbC5jaGlsZHJlbiwgbWVzc2FnZXMsIGVycm9ycyk7XG5cbiAgcmV0dXJuIChlcnJvcnMubGVuZ3RoID09IDApID9cbiAgICAgICAgICAgICBuZXcgWG1iRGVzZXJpYWxpemF0aW9uUmVzdWx0KG5vcm1hbGl6ZWRDb250ZW50LCBtZXNzYWdlcywgW10pIDpcbiAgICAgICAgICAgICBuZXcgWG1iRGVzZXJpYWxpemF0aW9uUmVzdWx0KG51bGwsIDx7W2tleTogc3RyaW5nXTogSHRtbEFzdFtdfT57fSwgZXJyb3JzKTtcbn1cblxuZnVuY3Rpb24gX2NoZWNrUm9vdEVsZW1lbnQobm9kZXM6IEh0bWxBc3RbXSk6IGJvb2xlYW4ge1xuICByZXR1cm4gbm9kZXMubGVuZ3RoIDwgMSB8fCAhKG5vZGVzWzBdIGluc3RhbmNlb2YgSHRtbEVsZW1lbnRBc3QpIHx8XG4gICAgICAgICAoPEh0bWxFbGVtZW50QXN0Pm5vZGVzWzBdKS5uYW1lICE9IF9CVU5ETEVfRUxFTUVOVDtcbn1cblxuZnVuY3Rpb24gX2NyZWF0ZU1lc3NhZ2VzKG5vZGVzOiBIdG1sQXN0W10sIG1lc3NhZ2VzOiB7W2tleTogc3RyaW5nXTogSHRtbEFzdFtdfSxcbiAgICAgICAgICAgICAgICAgICAgICAgICBlcnJvcnM6IFBhcnNlRXJyb3JbXSk6IHZvaWQge1xuICBub2Rlcy5mb3JFYWNoKChpdGVtKSA9PiB7XG4gICAgaWYgKGl0ZW0gaW5zdGFuY2VvZiBIdG1sRWxlbWVudEFzdCkge1xuICAgICAgbGV0IG1zZyA9IDxIdG1sRWxlbWVudEFzdD5pdGVtO1xuXG4gICAgICBpZiAobXNnLm5hbWUgIT0gX01TR19FTEVNRU5UKSB7XG4gICAgICAgIGVycm9ycy5wdXNoKFxuICAgICAgICAgICAgbmV3IFhtYkRlc2VyaWFsaXphdGlvbkVycm9yKGl0ZW0uc291cmNlU3BhbiwgYFVuZXhwZWN0ZWQgZWxlbWVudCBcIiR7bXNnLm5hbWV9XCJgKSk7XG4gICAgICAgIHJldHVybjtcbiAgICAgIH1cblxuICAgICAgbGV0IGlkID0gX2lkKG1zZyk7XG4gICAgICBpZiAoaXNCbGFuayhpZCkpIHtcbiAgICAgICAgZXJyb3JzLnB1c2goXG4gICAgICAgICAgICBuZXcgWG1iRGVzZXJpYWxpemF0aW9uRXJyb3IoaXRlbS5zb3VyY2VTcGFuLCBgXCIke19JRF9BVFRSfVwiIGF0dHJpYnV0ZSBpcyBtaXNzaW5nYCkpO1xuICAgICAgICByZXR1cm47XG4gICAgICB9XG5cbiAgICAgIG1lc3NhZ2VzW2lkXSA9IG1zZy5jaGlsZHJlbjtcbiAgICB9XG4gIH0pO1xufVxuXG5mdW5jdGlvbiBfaWQoZWw6IEh0bWxFbGVtZW50QXN0KTogc3RyaW5nIHtcbiAgbGV0IGlkcyA9IGVsLmF0dHJzLmZpbHRlcihhID0+IGEubmFtZSA9PSBfSURfQVRUUik7XG4gIHJldHVybiBpZHMubGVuZ3RoID4gMCA/IGlkc1swXS52YWx1ZSA6IG51bGw7XG59XG5cbmZ1bmN0aW9uIF9zZXJpYWxpemVNZXNzYWdlKG06IE1lc3NhZ2UpOiBzdHJpbmcge1xuICBsZXQgZGVzYyA9IGlzUHJlc2VudChtLmRlc2NyaXB0aW9uKSA/IGAgZGVzYz0nJHttLmRlc2NyaXB0aW9ufSdgIDogXCJcIjtcbiAgcmV0dXJuIGA8bXNnIGlkPScke2lkKG0pfScke2Rlc2N9PiR7bS5jb250ZW50fTwvbXNnPmA7XG59XG5cbmZ1bmN0aW9uIF9leHBhbmRQbGFjZWhvbGRlcihpbnB1dDogc3RyaW5nKTogc3RyaW5nIHtcbiAgcmV0dXJuIFJlZ0V4cFdyYXBwZXIucmVwbGFjZUFsbChfUExBQ0VIT0xERVJfUkVHRVhQLCBpbnB1dCwgKG1hdGNoKSA9PiB7XG4gICAgbGV0IG5hbWVXaXRoUXVvdGVzID0gbWF0Y2hbMl07XG4gICAgcmV0dXJuIGA8cGggbmFtZT0ke25hbWVXaXRoUXVvdGVzfT48L3BoPmA7XG4gIH0pO1xufVxuIl19