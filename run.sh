#!/usr/bin/env bash

# launcher for immutable distros, this also works for any distro but it was made because of imutable ones
# this also setups the dotnet if not already installed
# i am not very good at bash so i apologize if something is wrong or looking ugly

set -euo pipefail

DIR="$(cd "$(dirname "$(readlink -f "$0")")" && pwd)"
APP="$DIR/MediEvil"
CHANNEL="10.0"
NEEDED="Microsoft.NETCore.App 10."
LOCAL_ROOT="${DOTNET_INSTALL_DIR:-$HOME/.dotnet}"

say() { printf '\033[1;36m[launcher]\033[0m %s\n' "$1"; }
die() { printf '\033[1;31m[launcher] %s\033[0m\n' "$1" >&2; exit 1; }

#check if not already installed
has_runtime() {
    [ -n "${1:-}" ] && [ -x "$1/dotnet" ] || return 1
    "$1/dotnet" --list-runtimes 2>/dev/null | grep -qF "$NEEDED"
}

find_runtime() { #chcks locs
    if has_runtime "${DOTNET_ROOT:-}"; then
        echo "$DOTNET_ROOT"; return 0
    fi
    if has_runtime "$LOCAL_ROOT"; then
        echo "$LOCAL_ROOT"; return 0
    fi
    if command -v dotnet >/dev/null 2>&1; then
        local sys
        sys="$(dirname "$(readlink -f "$(command -v dotnet)")")"
        if has_runtime "$sys"; then echo "$sys"; return 0; fi
    fi
    for c in /usr/share/dotnet /usr/lib/dotnet /opt/dotnet; do
        if has_runtime "$c"; then echo "$c"; return 0; fi
    done
    return 1
}

install_runtime() {
    local script
    script="$(mktemp)"
    trap 'rm -f "$script"' RETURN

    say "downloading dotnet installer"
    if command -v curl >/dev/null 2>&1; then
        curl -fsSL https://dot.net/v1/dotnet-install.sh -o "$script" \
            || die "download failed, check your connection"
    elif command -v wget >/dev/null 2>&1; then
        wget -qO "$script" https://dot.net/v1/dotnet-install.sh \
            || die "download failed, check your connection"
    else
        die "neither curl nor wget is available, please install one of them"
    fi

    say "installing the .NET $CHANNEL runtime into $LOCAL_ROOT (this will take a while)"
    bash "$script" --channel "$CHANNEL" --runtime dotnet \
        --install-dir "$LOCAL_ROOT" --no-path \
        || die "dotnet install script failed"
}

ROOT="$(find_runtime || true)"

if [ -z "$ROOT" ]; then
    say ".NET $CHANNEL runtime not found"
    install_runtime
    ROOT="$(find_runtime || true)"
    [ -n "$ROOT" ] || die "the runtime is still missing after installing, this is an error, aborting"
    say "runtime ready"
fi

export DOTNET_ROOT="$ROOT"
export PATH="$ROOT:$PATH"

[ -x "$APP" ] || chmod +x "$APP" 2>/dev/null || true
[ -x "$APP" ] || die "MediEvil not found next to this script"

exec "$APP" "$@"
