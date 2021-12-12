

from collections import deque
import fileinput


input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
graph = {}
for line in input:
    start = line.split('-')[0]
    end = line.split('-')[1]
    if start not in graph:
        graph[start] = set()
    graph[start].add(end)
    if end not in graph:
        graph[end] = set()
    graph[end].add(start)

#print(graph)

def findpaths(graph, start, end, partTwo):
    # Create a queue which stores
    # the paths
    queue = deque()
    count = 0

    path = []
    path.append(start)
    queue.append(path.copy())
     
    while queue:
        path = queue.popleft()
        last = path[-1]
 
        if (last == end):
            count += 1
            #print(path)
            continue

        for node in graph[last]:
            if (node.isupper() or node not in path 
                    or (partTwo and (node != start and not any(element.islower() and path.count(element) > 1 for element in path)))):
                newpath = path.copy()
                newpath.append(node)
                queue.append(newpath)

    return count

count1 = findpaths(graph, "start", "end", False)
print(count1)
count1 = findpaths(graph, "start", "end", True)
print(count1)