def get_build_url(project, definition, branch):
    return f"https://dev.azure.com/{project}/_apis/build/builds?definitions={definition}&branchName={branch.replace('/', '%2F')}&resultFilter=succeeded&statusFilter=completed&queryOrder=queueTimeDescending&api-version=6.0"
    
def get_pr_url(project, repositoryId, branch):
    return f"https://dev.azure.com/{project}/_apis/git/repositories/{repositoryId}/pullRequests?searchCriteria.status=completed&searchCriteria.targetRefName={branch.replace('/', '%2F')}&api-version=6.0&queryOrder=queueTimeDescending"