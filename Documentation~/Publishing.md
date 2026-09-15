# Publishing

This repository is a Unity Package Manager package, not a complete Unity project. Keep `package.json`, `Editor`, `Tests`, `Documentation~`, and the root documentation files at repository root.

For each release:

1. Update `package.json` using semantic versioning.
2. Add the release notes to `CHANGELOG.md`.
3. Run EditMode tests in supported Unity versions.
4. Run `npm pack --dry-run` and inspect the included files.
5. Merge the release commit into `main`.
6. Create a matching annotated tag, such as `v1.0.1`.
7. Push both `main` and the tag.

Never move an already published version tag. Publish a new patch version instead.
